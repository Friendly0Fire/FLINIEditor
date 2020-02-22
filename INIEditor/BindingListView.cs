using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace INIEditor
{
	public class BindingListView<T> : IBindingListView where T : IComparable<T>
	{
		public delegate IComparer<T> ComparerGet(PropertyDescriptor property, ListSortDirection direction);
		private List<T> list;
		private List<T> publist;

		private List<ListChangedEventHandler> evs;

		private PropertyDescriptor property;
		private ListSortDirection direction;
		private bool sorted;
		private bool filtered;

		private Expression filter;
		private string filterStr;
		private List<string> filterValues;

		private ComparerGet singleSortComparer;

		public BindingListView(List<T> list)
		{
			this.list = list;
			this.publist = new List<T>(list);
			evs = new List<ListChangedEventHandler>();
			singleSortComparer = DefaultComparer;
		}

		public BindingListView(List<T> list, ComparerGet comparer)
		{
			this.list = list;
			this.publist = new List<T>(list);
			evs = new List<ListChangedEventHandler>();
			singleSortComparer = comparer;
		}

		private IComparer<T> DefaultComparer(PropertyDescriptor property, ListSortDirection direction)
		{
			return new PropertyComparer<T>(property, direction);
		}

		#region IBindingListView Members

		public void ApplySort(ListSortDescriptionCollection sorts)
		{
			throw new NotImplementedException();
		}

		public string Filter
		{
			get
			{
				return filterStr;
			}
			set
			{
				if (value != null && value.Trim() != "")
				{
					filterStr = value;
					filter = new Expression();
					ParseSubexpression(value, filter);
					filter.Complete();
					filterValues = new List<string>();
					filter.CompileValues(filterValues);

					#if DEBUG
						filter.Debug();
					#endif
				}
				else
				{
					filterValues = null;
					filter = null;
					filterStr = "";
				}

				RebuildPublicList();
			}
		}

		public void RemoveFilter()
		{
			filterValues = null;
			filter = null;
			filterStr = "";
			RebuildPublicList();
		}

		public ListSortDescriptionCollection SortDescriptions
		{
			get { throw new NotImplementedException(); }
		}

		public bool SupportsAdvancedSorting
		{
			get { return false; }
		}

		public bool SupportsFiltering
		{
			get { return true; }
		}

		#endregion

		#region IBindingList Members

		public void AddIndex(PropertyDescriptor property)
		{
			//throw new NotImplementedException();
		}

		public object AddNew()
		{
			throw new NotImplementedException();
		}

		public bool AllowEdit
		{
			get { return true; }
		}

		public bool AllowNew
		{
			get { return false; }
		}

		public bool AllowRemove
		{
			get { return true; }
		}

		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			if (publist != null)
			{
				IComparer<T> pc = singleSortComparer(property, direction);
				this.property = property;
				this.direction = direction;
				publist.Sort(pc);
				sorted = true;
			}
			else
				sorted = false;
		}

		public int Find(PropertyDescriptor property, object key)
		{
			throw new NotImplementedException();
		}

		public bool IsSorted
		{
			get { return sorted; }
		}

		public event ListChangedEventHandler ListChanged
		{
			add { evs.Add(value); }
			remove { evs.Remove(value); }
		}

		public void RemoveIndex(PropertyDescriptor property)
		{
			//throw new NotImplementedException();
		}

		public void RemoveSort()
		{
			sorted = false;
		}

		public ListSortDirection SortDirection
		{
			get { return direction; }
		}

		public PropertyDescriptor SortProperty
		{
			get { return property; }
		}

		public bool SupportsChangeNotification
		{
			get { return true; }
		}

		public bool SupportsSearching
		{
			get { return false; }
		}

		public bool SupportsSorting
		{
			get { return true; }
		}

		#endregion

		#region IList Members

		int System.Collections.IList.Add(object value)
		{
			list.Add((T)value);
			publist.Add((T)value);
			DoListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, publist.Count - 1));
			return Reposition(publist.Count - 1);
		}

		public int Add(T value)
		{
			list.Add(value);
			publist.Add(value);
			DoListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, publist.Count - 1));
			return Reposition(publist.Count - 1);
		}

		public void Clear()
		{
			publist.Clear();
			list.Clear();
			DoListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}

		bool System.Collections.IList.Contains(object value)
		{
			return publist.Contains((T)value);
		}

		public bool Contains(T value)
		{
			return publist.Contains(value);
		}

		int System.Collections.IList.IndexOf(object value)
		{
			return publist.IndexOf((T)value);
		}

		public int IndexOf(T value)
		{
			return publist.IndexOf(value);
		}

		void System.Collections.IList.Insert(int index, object value)
		{
			int i = list.IndexOf(publist[index]);
			if (i == -1) i = list.Count - 1;

			publist.Insert(index, (T)value);
			list.Insert(i, (T)value);
			DoListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));

			Reposition(index);
		}

		public void Insert(int index, T value)
		{
			int i = list.IndexOf(publist[index]);
			if (i == -1) i = list.Count - 1;

			publist.Insert(index, value);
			list.Insert(i, value);
			DoListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));

			Reposition(index);
		}

		public bool IsFixedSize
		{
			get { return false; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		void System.Collections.IList.Remove(object value)
		{
			int i = publist.IndexOf((T)value);
			list.Remove((T)value);

			if (publist.Remove((T)value))
				DoListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, i));
		}

		public void Remove(T value)
		{
			int i = publist.IndexOf((T)value);
			list.Remove((T)value);

			if (publist.Remove((T)value))
				DoListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, i));
		}

		public void RemoveAt(int index)
		{
			list.Remove(publist[index]);
			publist.RemoveAt(index);
			DoListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
		}

		object System.Collections.IList.this[int index]
		{
			get
			{
				return publist[index];
			}
			set
			{
				int i = list.IndexOf(publist[index]);
				publist[index] = (T)value;
				list[i] = (T)value;
				DoListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));

				Reposition(index);
			}
		}

		public T this[int index]
		{
			get
			{
				return publist[index];
			}
			set
			{
				int i = list.IndexOf(publist[index]);
				publist[index] = (T)value;
				list[i] = (T)value;
				DoListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));

				Reposition(index);
			}
		}

		#endregion

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			publist.CopyTo((T[])array, index);
		}

		public int Count
		{
			get { return publist.Count; }
		}

		public bool IsSynchronized
		{
			get { return false; }
		}

		public object SyncRoot
		{
			get { return publist; }
		}

		#endregion

		#region IEnumerable Members

		public System.Collections.IEnumerator GetEnumerator()
		{
			return publist.GetEnumerator();
		}

		#endregion

		private int Reposition(int index)
		{
			if (filtered)
			{
				if (!MatchesFilter(publist[index]))
				{
					publist.RemoveAt(index);
					DoListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
					return -1;
				}
			}
			if (sorted)
			{
				PropertyComparer<T> pc = new PropertyComparer<T>(property, direction);
				T v = publist[index];
				publist.RemoveAt(index);

				int next = 1, offset = 0;
				do
				{
					next = pc.Compare(v, publist[offset]);
					offset += next;
					if (offset >= publist.Count || offset < 0) break;
				} while (next != 0);

				if (offset < 0)
				{
					publist.Insert(0, v);
					DoListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, 0, index));
					return 0;
				}
				else if (offset >= publist.Count)
				{
					publist.Add(v);
					DoListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, publist.Count - 1, index));
					return publist.Count - 1;
				}
				else
				{
					publist.Insert(offset, v);
					DoListChanged(new ListChangedEventArgs(ListChangedType.ItemMoved, offset, index));
					return offset;
				}
			}
			return index;
		}

		private void RebuildPublicList()
		{
			if (filter != null)
			{
				publist.Clear();
				publist.AddRange(list.FindAll(x => MatchesFilter(x)));
				filtered = true;
			}
			else
			{
				filtered = false;
				publist.Clear();
				publist.AddRange(list);
			}

			if (sorted) ApplySort(property, direction);

			DoListChanged(new ListChangedEventArgs(ListChangedType.Reset, 0));
		}

		private bool MatchesFilter(T element)
		{
			if (element is IFilterable)
			{
				IFilterable f = element as IFilterable;
				return filter.Execute(f.GetFilterDictionary(filterValues));
			}

			return false;
		}

		private void DoListChanged(ListChangedEventArgs e)
		{
			foreach (ListChangedEventHandler h in evs)
			{
				h(this, e);
			}
		}

		public interface IFilterable
		{
			Dictionary<string, string> GetFilterDictionary(List<string> values);
		}

		private int ParseSubexpression(string expression, Expression parent)
		{
			Expression e = new Expression();
			parent.Add(e);

			int n = 0;

			for (int a = 0; a < expression.Length; )
			{
				switch (expression[a])
				{
					case '(':
						if (n < a)
							e.Add(new Expression(expression.Substring(n, a - n)));
						a = ParseSubexpression(expression.Substring(a + 1), e) + a + 1;
						n = a;
						break;
					case ')':
						if (n < a)
							e.Add(new Expression(expression.Substring(n, a - n)));
						return a + 1;
					case '&':
						if (expression[a + 1] != '&') goto default;

						if (n < a)
							e.Add(new Expression(expression.Substring(n, a - n)));
						e.next = BindingListView<T>.Expression.Operator.AND;
						e = new Expression();
						parent.Add(e);
						a += 2;
						n = a;
						break;
					case '|':
						if (expression[a + 1] != '|') goto default;

						if (n < a)
							e.Add(new Expression(expression.Substring(n, a - n)));
						e.next = BindingListView<T>.Expression.Operator.OR;
						e = new Expression();
						parent.Add(e);
						a += 2;
						n = a;
						break;
					default:
						a++;
						break;
				}
			}
			if (n < expression.Length)
				e.Add(new Expression(expression.Substring(n)));

			return expression.Length;
		}

		private class Comparator
		{
			private string data, value;
			private double dValue;
            private long iValue;
			private bool isNum, isInt, isUInt;
			private Comparison com;

			public Comparator(string expression)
			{
				expression = expression.Trim();
				string[] v = new string[0];
				if (expression.IndexOf("<=") != -1)
				{
					v = expression.Split(new string[] { "<=" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.LEQ;
				}
				else if (expression.IndexOf(">=") != -1)
				{
					v = expression.Split(new string[] { ">=" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.GEQ;
				}
				else if (expression.IndexOf("<") != -1)
				{
					v = expression.Split(new string[] { "<" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.LT;
				}
				else if (expression.IndexOf(">") != -1)
				{
					v = expression.Split(new string[] { ">" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.GT;
				}
				else if (expression.IndexOf("==") != -1)
				{
					v = expression.Split(new string[] { "==" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.EQ;
				}
				else if (expression.IndexOf("!=") != -1)
				{
					v = expression.Split(new string[] { "!=" }, StringSplitOptions.None);
					com = BindingListView<T>.Comparator.Comparison.NEQ;
				}

				if (v.Length == 2)
				{
					data = v[0].Trim().ToLower();
					value = v[1].Trim().ToLower();
				}
				else if (v.Length == 0)
				{
					if (expression[0] == '!')
					{
						expression = expression.Substring(1);
						com = Comparison.NEQ;
					}
					else
						com = Comparison.EQ;

					data = expression.Trim().ToLower();
					value = "true";

				}
				else throw new ArgumentException();

                isNum = Double.TryParse(value, out dValue);

                if (value.Substring(0, 2).ToLower() == "0x")
                    isInt = Int64.TryParse(value.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out iValue);
                else if (value.Substring(0, 2).ToLower() == "0u")
                {
                    isInt = Int64.TryParse(value.Substring(2), out iValue);
                    if (isInt && iValue < 0)
                        iValue += 1 >> 32;
                }
                else
                    isInt = Int64.TryParse(value, out iValue);

                isNum = isNum || isInt;
			}

			public bool Matches(string v)
			{
				if (isNum)
				{
                    if (isInt)
                    {
                        long i = 0;
                        if (!Int64.TryParse(v, out i)) return false;
                        switch (com)
                        {
                            case Comparison.EQ:
                                return i == iValue;
                            case Comparison.NEQ:
                                return i != iValue;
                            case Comparison.LEQ:
                                return i <= iValue;
                            case Comparison.GEQ:
                                return i >= iValue;
                            case Comparison.LT:
                                return i < iValue;
                            case Comparison.GT:
                                return i > iValue;
                        }
                    }
                    else
                    {
                        double d = 0;
                        if (!Double.TryParse(v, out d)) return false;
                        switch (com)
                        {
                            case Comparison.EQ:
                                return d == dValue;
                            case Comparison.NEQ:
                                return d != dValue;
                            case Comparison.LEQ:
                                return d <= dValue;
                            case Comparison.GEQ:
                                return d >= dValue;
                            case Comparison.LT:
                                return d < dValue;
                            case Comparison.GT:
                                return d > dValue;
                        }
                    }
				}

				switch (com)
				{
					case Comparison.EQ:
						return v.Equals(value, StringComparison.InvariantCultureIgnoreCase);
					case Comparison.NEQ:
						return !v.Equals(value, StringComparison.InvariantCultureIgnoreCase);
					case Comparison.LEQ:
						return v.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
					case Comparison.GEQ:
						return v.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
					case Comparison.LT:
						return !v.Equals(value, StringComparison.InvariantCultureIgnoreCase) && v.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
					case Comparison.GT:
						return !v.Equals(value, StringComparison.InvariantCultureIgnoreCase) && value.StartsWith(v, StringComparison.InvariantCultureIgnoreCase);
				}

				return false;
			}

			public string Data
			{
				get
				{
					return data;
				}
			}

			public enum Comparison
			{
				LEQ,
				GEQ,
				EQ,
				NEQ,
				LT,
				GT
			}

			public override string ToString()
			{
				return data + " " + com + " " + value;
			}
		}

		private class Expression
		{
			private List<Expression> children;
			public Operator next;
			public State Early = State.NONE;

			public Comparator inner;

			public Expression()
			{
				children = new List<Expression>();
				next = BindingListView<T>.Expression.Operator.NONE;
				inner = null;
			}

			public Expression(string v)
			{
				inner = new Comparator(v);
				children = null;
				next = BindingListView<T>.Expression.Operator.NONE;
			}

			public void Add(Expression e)
			{
				children.Add(e);
			}

			public bool Execute(Dictionary<string, string> values)
			{
				bool result = true;
				Operator prev = Operator.NONE;

				if (children == null)
				{
					if (values.ContainsKey(this.inner.Data))
						return this.inner.Matches(values[this.inner.Data]);
					else return false;
				}
				else
				{
					foreach (Expression e in children)
					{
						bool r = e.Execute(values);

						if (prev == Operator.NONE)
							result = r;
						else if (prev == Operator.AND)
							result = result && r;
						else if (prev == Operator.OR)
							result = result || r;

						if (e.Early == State.ALL_AND && result == false) return false;
						if (e.Early == State.ALL_OR && result == true) return true;

						prev = e.next;
					}

					return result;
				}
			}

			public void Complete()
			{
				if (children == null) return;

				bool allAnds = true, allOrs = true;

				foreach (Expression e in children)
				{
					if (e.next == Operator.AND)
						allOrs = false;
					else if (e.next == Operator.OR)
						allAnds = false;

					if (!allAnds && !allOrs) break;
				}
				foreach (Expression e in children)
				{
					if (allAnds) e.Early = State.ALL_AND;
					else if (allOrs) e.Early = State.ALL_OR;

					e.Complete();
				}
			}

			public void CompileValues(List<string> values)
			{
				if (inner != null)
					values.Add(inner.Data);
				if (children != null)
				{
					foreach (Expression e in children)
						e.CompileValues(values);
				}
			}

			public enum Operator
			{
				AND,
				OR,
				NONE
			}

			public enum State
			{
				ALL_AND,
				ALL_OR,
				NONE
			}

			public void Debug()
			{
				if (children != null)
				{
					System.Diagnostics.Debug.WriteLine("Node, children:");
					System.Diagnostics.Debug.Indent();
					foreach (Expression e in children)
						e.Debug();
					System.Diagnostics.Debug.WriteLine("Next OP = " + next);
					System.Diagnostics.Debug.Unindent();
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("Node, comparator:");
					System.Diagnostics.Debug.Indent();
					System.Diagnostics.Debug.WriteLine(inner.ToString());
					System.Diagnostics.Debug.Unindent();
				}
			}
		}
	}
}
