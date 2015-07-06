using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using Orange.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Orange.Core.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectBuilder
    {
        // this populates a SINGLE, simple object
        public static object PopulateBasicObject<T>(T objectType, DataTable objectProperties)
        {
            object objectBeingBuilt = Activator.CreateInstance(objectType.GetType());
            var properties = objectType.GetType().GetProperties(); // this is here as the order is random, and if we do it once it solves that problem
            foreach (DataRow row in objectProperties.Rows)
            {
                List<object> items = row.ItemArray.ToList();
                PopulateProperties(objectType, ref objectBeingBuilt, items, properties);
            }
            return objectBeingBuilt;              
        }

        public static List<object> PopulateMulitpleBasicObjects<T>(T objectType, DataTable objectProperties)
        {
            List<object> objectsToBeBuilt = new List<object>();
            var properties = objectType.GetType().GetProperties(); // this is here as the order is random, and if we do it once it solves that problem
            foreach (DataRow row in objectProperties.Rows)
            {
                object objectBeingBuilt = Activator.CreateInstance(objectType.GetType());
                PopulateProperties(objectType, ref objectBeingBuilt, row.ItemArray.ToList(), properties);
                objectsToBeBuilt.Add(objectBeingBuilt);
            }
            return objectsToBeBuilt;
        }

        /// <summary>
        /// Populate an object from the datasource which contains a list of other objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectType"></param>
        /// <param name="objectProperties"></param>
        /// <returns></returns>
        public static List<object> PopulateObjectWithNestedObject<T>(T objectType, List<DataTable> objectProperties)
        {
            List<object> listOfObjects = new List<object>();

            foreach (DataRow row in objectProperties.First().Rows)
            {
                object objectBeingBuilt = Activator.CreateInstance(objectType.GetType());
                List<object> items = row.ItemArray.ToList();

                int count = 0;                
                int subObjectCount = objectProperties.Count - 1; // the number of objects within the object we need to construct
                foreach (PropertyInfo property in objectType.GetType().GetProperties())
                {
                    // handles List<int> cases or Dictionaries
                    if (property.PropertyType.Name == "List`1" | property.PropertyType.Name == "Dictionary`2")
                    {
                        bool assignedToList = PopulateObjectLists(property, objectBeingBuilt, row, objectProperties.Last(), ref subObjectCount);
                        if (assignedToList) continue;
                        count++;
                    }
                    else
                    {
                        if (count == items.Count)
                        {
                            objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(1, items[count], null);
                        }
                        else
                        {
                            objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, items[count], null);
                        }
                        count++;
                    }
                }
                listOfObjects.Add(objectBeingBuilt);
            }
            return listOfObjects;
        }

        /// <summary>
        /// Populate an object's List<> properties with values returned from the database
        /// </summary>
        /// <param name="property"></param>
        /// <param name="objectBeingBuilt"></param>
        /// <param name="row"></param>
        /// <param name="additionalData"></param>
        /// <param name="subObjectCount">Indicates the number of objects within the parent object. If zero we need to add an empty list.</param>
        /// <returns></returns>
        private static bool PopulateObjectLists(PropertyInfo property, object objectBeingBuilt, DataRow row, DataTable additionalData, ref int subObjectCount)
        {
            //if (objectBeingBuilt.GetType().Name == "MyClass")
            //{
            //    // this section is all an example really
            //    List<int> listOfInts = new List<int>();
            //    if(row.ItemArray.Any())
            //    {
            //        // TODO: the "1000" below if an extra marker that can be in place to look for something. Changing the signature will need to be done to accomodate this
            //        listOfInts = additionalData.AsEnumerable().Where(t => t.Field<int>("FieldNameFromDataSource") == 1000).Select(d => Convert.ToInt32(d.ItemArray[1])).ToList();
            //        objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, listOfInts, null);
            //    }
            //    else
            //    {
            //        listOfInts.Add(0);
            //    }
            //    return true;
            //}
            if (objectBeingBuilt.GetType().Name == "Post")
            {
                if (subObjectCount == 0)
                {
                    objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, new List<Tag>(), null);
                    return true;
                }
                if(row.ItemArray.Any())
                {
                    List<Tag> postTags = new List<Tag>(PopulateMulitpleBasicObjects(new Tag(), additionalData).Cast<Tag>());
                    objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, postTags, null);
                    --subObjectCount;
                }
                return true;
            }
            return false;
        }

        private static void PopulateProperties(object objectType, ref object objectBeingBuilt, List<object> items, PropertyInfo[] properties)
        {
            bool inheritedObject = false;
            int basePropertyCount = 0;
            if (objectType.GetType().BaseType.Name != objectType.GetType().Name)
            {
                basePropertyCount = Math.Abs(objectType.GetType().BaseType.GetProperties().Length - objectType.GetType().GetProperties().Length);
                inheritedObject = true;
            }

            int count = 0;
            foreach (PropertyInfo property in properties)
            {
                if (count == items.Count)
                {
                    // I don't know what this was here
                    //objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(1, items[count], null);
                    break;
                }
                else
                {
                    if (inheritedObject)
                    {
                        if (count >= basePropertyCount)
                        {
                            objectBeingBuilt.GetType().BaseType.GetProperty(property.Name).SetValue(objectBeingBuilt, items[count], null);
                        }
                        else
                        {
                            objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, items[count], null);
                        }
                    }
                    else
                    {
                        objectBeingBuilt.GetType().GetProperty(property.Name).SetValue(objectBeingBuilt, items[count], null);
                    }
                }
                count++;
            }
        }
    }
}
