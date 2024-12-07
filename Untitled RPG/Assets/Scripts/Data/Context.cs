using System.Collections.Generic;
using UnityEngine;

namespace RPG.Data
{
    public interface IContextProvider
    {
         Context GetContext();
    }
    public class Context 
    {
        public Transform Transform { get; set; }

        // Dictionary to store instance-specific data based on its type
        private Dictionary<System.Type, object> _data = new Dictionary<System.Type, object>();

        // Method to get data from the context (for instance-specific storage)
        public bool TryGetData<T>(out T data) where T : class
        {
            // Check if the data of the specified type exists in the dictionary
            if (_data.TryGetValue(typeof(T), out var storedData))
            {
                data = storedData as T;
                return true;
            }

            data = null;
            return false;
        }

        // Method to set data in the context
        public void SetData<T>(T data) where T : class
        {
            var type = typeof(T);
            // If the data already exists, update it; otherwise, add it
            if (_data.ContainsKey(type))
            {
                _data[type] = data;
            }
            else
            {
                _data.Add(type, data);
            }
        }
    }
}
