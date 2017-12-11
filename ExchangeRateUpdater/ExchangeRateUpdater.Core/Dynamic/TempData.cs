namespace ExchangeRateUpdater.Core.Dynamic {
	using System.Collections.Generic;
	using System.Dynamic;

	public class TempData : DynamicObject {
		public TempData() { }
		// The inner dictionary.
		Dictionary<string, object> _dictionary
			= new Dictionary<string, object>();

		// This property returns the number of elements
		// in the inner dictionary.
		public int Count {
			get {
				return _dictionary.Count;
			}
		}

		// If you try to get a value of a property 
		// not defined in the class, this method is called.
		public override bool TryGetMember(
			GetMemberBinder binder, out object result) {
			// Converting the property name to lowercase
			// so that property names become case-insensitive.
			string name = binder.Name.ToLower();

			// If the property name is found in a dictionary,
			// set the result parameter to the property value and return true.
			// Otherwise, return false.
			return _dictionary.TryGetValue(name, out result);
		}

		// If you try to set a value of a property that is
		// not defined in the class, this method is called.
		public override bool TrySetMember(
			SetMemberBinder binder, object value) {
			// Converting the property name to lowercase
			// so that property names become case-insensitive.
			_dictionary[binder.Name.ToLower()] = value;

			// You can always add a value to a dictionary,
			// so this method always returns true.
			return true;
		}

		public void Clear() {
			_dictionary.Clear();
		}
	}
}
