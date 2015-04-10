//using System;
//using System.Collections.Generic;
//using System.Windows.Data;
//using ManagementGui.View.TreeViewUserAndTasks.Model;
//namespace ManagementGui.View.TreeViewUserAndTasks.Common
//{
//    public class HierarchyViewModel : ViewModelBase
//    {
//        public CollectionView Customers { get; private set; }
//        private HierarchyItemViewModel _selectedItem;


//        public HierarchyViewModel(List<UserTrV> users, object selectedEntity)
//        {
//            // this is an alternative more generic and more compact version of the code below - it is however a little more tricky to understand
//            //var customerHierarchyItemsList = BuildHierarchyList<Customer>(users, ((a, b) => a.Number == b.Number), selectedEntity,
//            //    x => BuildHierarchyList<Order>(x.Orders, ((a, b) => a.Number == b.Number), selectedEntity,
//            //        y => BuildHierarchyList<Product>(y.Products, ((a, b) => a.GetHashCode() == b.GetHashCode()), selectedEntity, null)
//            //    )
//            //);

//            // create the top level collectionview for the users
//            var customerHierarchyItemsList = new List<HierarchyItemViewModel>();

//            foreach (UserTrV c in users)
//            {
//                // create the hierarchy item and add to the list
//                var customerHierarchyItem = new HierarchyItemViewModel(c);
//                customerHierarchyItemsList.Add(customerHierarchyItem);

//                // check if this is the selected item
//                if (selectedEntity != null && selectedEntity.GetType() == typeof(UserTrV) && (selectedEntity as UserTrV).Equals(c))
//                {
//                    _selectedItem = customerHierarchyItem;
//                }

//                // if there are any orders in customerHierarchyItem
//                if (c.Orders.Count != 0)
//                {
//                    // create a new list of HierarchyItems
//                    var orderHierarchyItemsList = new List<HierarchyItemViewModel>();
//                    // loop through the orders and add them
//                    foreach (Order o in c.Orders)
//                    {
//                        // create the hierarchy item and add to the list
//                        var orderHierarchyItem = new HierarchyItemViewModel(o);
//                        orderHierarchyItem.Parent = customerHierarchyItem;
//                        orderHierarchyItemsList.Add(orderHierarchyItem);

//                        // check if this is the selected item
//                        if (selectedEntity != null && selectedEntity.GetType() == typeof(Order) && (selectedEntity as Order).Equals(o))
//                        {
//                            _selectedItem = orderHierarchyItem;
//                        }

//                        // if there are any products in orderHierarchyItem
//                        if (o.Products.Count != 0)
//                        {
//                            // create a new list of HierarchyItems
//                            var productHierarchyItemsList = new List<HierarchyItemViewModel>();
//                            // loop through the sites and add them
//                            foreach (Product p in o.Products)
//                            {
//                                // create the hierarchy item and add to the list
//                                var productHierarchyItem = new HierarchyItemViewModel(p);
//                                productHierarchyItem.Parent = orderHierarchyItem;
//                                productHierarchyItemsList.Add(productHierarchyItem);

//                                // check if this is the selected item
//                                if (selectedEntity != null && selectedEntity.GetType() == typeof(Product) && (selectedEntity as Product).Equals(p))
//                                {
//                                    _selectedItem = productHierarchyItem;
//                                }
//                            }

//                            // create the children of the order
//                            orderHierarchyItem.Children = new CollectionView(productHierarchyItemsList);
//                        }
//                    }
//                    // create the children of the customer
//                    customerHierarchyItem.Children = new CollectionView(orderHierarchyItemsList);
//                }
//            }

//            this.Customers = new CollectionView(customerHierarchyItemsList);

//            // select the selected item and expand it's parents
//            if (_selectedItem != null)
//            {
//                _selectedItem.IsSelected = true;
//                HierarchyItemViewModel current = _selectedItem.Parent;

//                while (current != null)
//                {
//                    current.IsExpanded = true;
//                    current = current.Parent;
//                }
//            }
//        }

//        private List<HierarchyItemViewModel> BuildHierarchyList<T>(List<T> sourceList, Func<T, T, bool> isSelected, object selectedEntity, Func<T, List<HierarchyItemViewModel>> getChildren)
//        {
//            List<HierarchyItemViewModel> result = new List<HierarchyItemViewModel>();

//            foreach (T item in sourceList)
//            {
//                // create the hierarchy item and add to the list
//                var hierarchyItem = new HierarchyItemViewModel(item);
//                result.Add(hierarchyItem);

//                // check if this is the selected item
//                if (selectedEntity != null && selectedEntity.GetType() == typeof(T) && (isSelected.Invoke(item, (T)selectedEntity)))
//                {
//                    _selectedItem = hierarchyItem;
//                }

//                if (getChildren != null)
//                {
//                    var children = getChildren.Invoke(item);
//                    children.ForEach(x => x.Parent = hierarchyItem);
//                    hierarchyItem.Children = new CollectionView(children);
//                }
//            }

//            return result;
//        }
//    }
//}
