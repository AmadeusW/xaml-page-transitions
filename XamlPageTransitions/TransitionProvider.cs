using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XamlPageTransitions
{
    static class TransitionProvider
    {
        static bool _initialized;
        static Dictionary<string, Type> _transitionTypes;
        static Dictionary<string, int> _selectedIndices;

        private static void initialize()
        {
            var transitionBaseType = Type.GetType("Windows.UI.Xaml.Media.Animation.Transition, Windows, ContentType=WindowsRuntime");
            var transitionTypes = from t in transitionBaseType.GetTypeInfo().Assembly.GetTypes()
                              where t != transitionBaseType && transitionBaseType.IsAssignableFrom(t)
                              select t;

            _transitionTypes = transitionTypes.ToDictionary(t => t.Name, t => t);
            _selectedIndices = new Dictionary<string, int>();
            _initialized = true;
        }

        public static IEnumerable<string> TransitionNames
        {
            get
            {
                if (!_initialized)
                    initialize();
                return _transitionTypes.Keys;
            }
        }

        internal static int GetSelectedIndex(string key)
        {
            int index;

            if (_selectedIndices.TryGetValue(key, out index))
            {
                return index;
            }
            return 0;
        }

        /// <summary>
        /// Gets the desired transition 
        /// and remembers the index for use by GetSelectedIndex
        /// </summary>
        /// <param name="key">Association for the index</param>
        /// <param name="transitionName">Desired transition name</param>
        /// <param name="selectionIndex">Index to store</param>
        /// <returns>Transition indicated by transitionName</returns>
        internal static Type SelectTransition(string key, string transitionName, int selectionIndex)
        {
            Type type;
            _selectedIndices[key] = selectionIndex;
            if (_transitionTypes.TryGetValue(transitionName, out type))
            {
                return type;
            }

            return null;
        }
    }
}
