using System.Collections.Generic;

namespace AKCCore {

    public static class ClippingStorage {
        public static List<Clipping> finalClippingsList;
        public static Dictionary<int, Clipping> numberedClippings;

        static ClippingStorage() {
            finalClippingsList = new List<Clipping>();
            numberedClippings = new Dictionary<int, Clipping>();
        }

        /// <summary>
        /// Adds a clipping to the storage and returns a unique identifier for later retrieval.
        /// </summary>
        /// <param name="clipping">The clipping to add to the storage</param>
        /// <returns>The unique identifier corresponding to this clipping</returns>

        public static int AddClipping(Clipping clipping) {
            int id = numberedClippings.Keys.Count;
            numberedClippings.Add(id, clipping);
            return id;
        }

        /// <summary>
        /// Retrieves a clipping from the storage.
        /// </summary>
        /// <param name="id">Unique identifier (obtained as the return result of the AddClipping method)</param>
        /// <returns>The clipping associated with the identifier</returns>

        public static Clipping GetClipping(int id) {
            return numberedClippings[id];
        }

        public static void ClearStorage() {
            finalClippingsList.Clear(); 
            numberedClippings.Clear();  
        }
    }
}