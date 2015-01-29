using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Northwoods.GoXam;
using Northwoods.GoXam.Model;
using Northwoods.GoXam.Tool;
using SilverlightTestApplication.Core;
//using System.Collections.ObjectModel;
using System.Collections;
using SilverlightTestApplication.ServiceReference1;

namespace SilverlightTestApplication
{
    public partial class ComplexERDiagram : UserControl
    {
        public ComplexERDiagram()
        {
            InitializeComponent();
                       
            guids = new ObservableCollection<string>() { 
                "46C86C68-42AE-4089-8398-6E4140FE8658",
                "F44C91CF-6EBD-4C44-8E55-98FDF89A87BC",
                "6FDE3427-EC80-4436-8553-11ECBDB0AB43",
                "CCAC642A-5CB9-4728-87DB-1A604889CA64"
            };
                     

            InitializeListBox();
           

            
            LoadInitialData(false);
            
        }

        private void InitializeListBox()
        {
            var ws = GetData.GetService();
            ws.LoadTypesCompleted += ws_LoadTypesCompleted;
            ws.LoadTypesAsync();
        }

        void ws_LoadTypesCompleted(object sender, LoadTypesCompletedEventArgs e)
        {
            if (e.Error != null)
                System.Windows.Browser.HtmlPage.Window.Alert(e.Error.Message);

            else
            {
                var listTypeData = e.Result;
                AllItems = listTypeData;
                myListBox.DataContext = listTypeData;
            }
        }
        public ObservableCollection<PairRelation> AllItems { get; private set; }


        private  void myListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null) return;

            var array = listBox.SelectedItems;           
        }

        private void UpdateData_Click(object sender, RoutedEventArgs e)
        {
            var collection = myListBox.SelectedItems;
           
            guids.Clear();
            foreach (var pair in collection)
            {
                PairRelation newpair = pair as PairRelation;
                if (newpair != null)
                {
                    guids.Add(newpair.Key1.ToString());
                }
            }

            LoadInitialData(false);
        }
        private void LoadInitialData(bool isTestMode = true)
        {            
            var model = new GraphLinksModel<EntityEx, String, String, Relationship>();
            model.NodesSource = new ObservableCollection<EntityEx>();
            model.LinksSource = new ObservableCollection<Relationship>();

            //model.HasUndoManager = true;
            myDiagram.Model = model;
            model.Modifiable = true;
            mainRelationCollection = new ObservableCollection<Relationship>();
            if (isTestMode)
            {
                #region [TESTDATA]
                List<EntityEx> collection = new List<EntityEx>(){
                    new EntityEx() { Key="Products",
                                   Items=new Attribute[] {
                                     new Attribute("ProductID", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("ProductName", false, NodeFigure.Cube1, "LightBlue"),
                                     new Attribute("SupplierID", false, NodeFigure.Decision, "Purple"),
                                     new Attribute("CategoryID", false, NodeFigure.Decision, "Purple") } },
                    new EntityEx() { Key="Computer", RootName="Products",
                                   Items=new Attribute[] {
                                     new Attribute("ComputerID", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("ComputerName", false, NodeFigure.Cube1, "LightBlue"),
                                     new Attribute("OSName", false, NodeFigure.Cube1, "LightBlue"),
                                     new Attribute("IPAddress", false, NodeFigure.Cube1, "LightBlue") } },
                    new EntityEx() { Key="Mobile",RootName="Products",
                                   Items=new Attribute[] {
                                     new Attribute("MobileID", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("MobileName", false, NodeFigure.Cube1, "LightBlue"),
                                     new Attribute("Description", false, NodeFigure.Cube1, "LightBlue"),
                                     new Attribute("Picture", false, NodeFigure.TriangleUp, "Red") } },
                    new EntityEx() { Key="Company", 
                                   Items=new Attribute[] {
                                     new Attribute("CompanyID", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("CompanyName", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("Contry", false, NodeFigure.MagneticData, "Green"),
                                     new Attribute("Asset", false, NodeFigure.MagneticData, "Green"),
                                     new Attribute("PersonCount", false, NodeFigure.MagneticData, "Green")}},                         
                    new EntityEx() { Key="Person", RootName="Company",
                                   Items=new Attribute[] {
                                     new Attribute("PersonID", true, NodeFigure.Decision, "Yellow"),
                                     new Attribute("Name", false, NodeFigure.Decision, "Green"),                       
                                     new Attribute("Age", false, NodeFigure.MagneticData, "Green") } },
                        };
                MainDataCollection = collection;
                #endregion
                CreateSubGraph(null);
            }
            else
            {
                GetDataCollection();
            }
        }


        ObservableCollection<string> guids = new ObservableCollection<string>();
        List<EntityEx> MainDataCollection = new List<EntityEx>();
        ObservableCollection<Relationship> mainRelationCollection = new ObservableCollection<Relationship>();



        Random rand = new Random();

        private void CreateSubGraph(EntityEx group)
        {
            ISubGraphModel sgmodel = myDiagram.Model as ISubGraphModel;
            if (sgmodel == null) 
                return;

            var nodes = MainDataCollection;

            if (group == null)
            {
                var rootCollection = nodes.Where(n => String.IsNullOrEmpty(n.RootName));
                foreach (EntityEx entity in rootCollection)
                {
                    entity.Color = String.Format("#{0:X}{1:X}{2:X}", 80 + rand.Next(100), 80 + rand.Next(100), 80 + rand.Next(100));
                    entity.IsSubGraph = true;
                    if (group != null)
                        entity.SubGraphKey = group.Key;
                    sgmodel.AddNode(entity);
                }

            //    GraphLinksModel<EntityEx, String, String, Relationship> model = myDiagram.Model as GraphLinksModel<EntityEx, String, String, Relationship>;
            //    model.LinksSource = new ObservableCollection<Relationship>() {
            //    new Relationship() { From="SVMEmailSignatureType", To="ASMComplexSystemType", Text="0..N", ToText="1" }
            //};

                GraphLinksModel<EntityEx, String, String, Relationship> model = myDiagram.Model as GraphLinksModel<EntityEx, String, String, Relationship>;
                model.LinksSource = mainRelationCollection;
            }
            else
            {
                var rootCollection = nodes.Where(n => n.RootName.Equals(group.Key));
                foreach (EntityEx entity in rootCollection)
                {
                    entity.Color = String.Format("#{0:X}{1:X}{2:X}", 80 + rand.Next(100), 80 + rand.Next(100), 80 + rand.Next(100));
                    entity.IsSubGraph = false;
                    if (group != null)
                        entity.SubGraphKey = group.Key;
                    sgmodel.AddNode(entity);
                }
            }
            myDiagram.InitialCenteredNodeData = sgmodel.NodesSource.OfType<EntityEx>().First();          
        }

        private void CollapseExpandButton_Click(object sender, RoutedEventArgs e)
        {          
            Button button = (Button)sender;
            Group sg = Part.FindAncestor<Group>(button);
            if (sg != null)
            {
                EntityEx subgraphdata = (EntityEx)sg.Data;
                if (!subgraphdata.IsSubGraph) return;           
                if (!subgraphdata.EverExpanded && !sg.IsExpandedSubGraph)
                {
                    subgraphdata.EverExpanded = true;  
                    CreateSubGraph(subgraphdata);          
                }
         
                sg.IsExpandedSubGraph = !sg.IsExpandedSubGraph;
                myDiagram.Panel.CenterPart(sg);
           
            }
        }
              
        private void GetDataCollection()
        {
            var ws = GetData.GetService();
            ws.GetDataUsingDataContractCompleted += ws_GetDataUsingDataContractCompleted;
            ws.GetDataUsingDataContractAsync(guids);          
        }

     
        void rel_GetRelationUsingDataContractCompleted(object sender, GetRelationUsingDataContractCompletedEventArgs e)
        {
            if (e.Error != null)
                System.Windows.Browser.HtmlPage.Window.Alert(e.Error.Message);

            else
            {
                var relationCollection = e.Result.ToList();


                List<PairRelation> revertCollection = new List<PairRelation>();

                foreach (var item in relationCollection)
                {
                    if (revertCollection.Any(p => p.Key1 == item.Key1 && p.Key2 == item.Key2))
                        continue;
                    if (revertCollection.Any(p => p.Key1 == item.Key2 && p.Key2 == item.Key1))
                        continue;
                    revertCollection.Add(item);
                }
               


                //List<PairRelation> reverCollection = new List<PairRelation>();
                //foreach (var item in relationCollection)
                //{
                //    reverCollection.Add(new PairRelation() { Key1 = item.Key2, Key2 = item.Key1 });
                //}
                //relationCollection.AddRange(reverCollection);
                //IEqualityComparer<PairRelation> lcomparer = new LambdaComparer<PairRelation>((p1, p2) => p1.Key1.ToString().Equals(p2.Key1.ToString()) && p1.Key2.ToString() == p2.Key2.ToString());
                //var lastCollection = relationCollection.Distinct(lcomparer);


                foreach (var item in revertCollection)
                {
                    mainRelationCollection.Add(new Relationship() { From=item.Key1, To=item.Key2, Text="N", ToText="M" });
                }               
            }

            CreateSubGraph(null);
        }

        void ws_GetDataUsingDataContractCompleted(object sender, ServiceReference1.GetDataUsingDataContractCompletedEventArgs e)
        {
            if (e.Error != null)
                System.Windows.Browser.HtmlPage.Window.Alert(e.Error.Message);

            else
            {
                var _tables = e.Result.Tables;
                UpdateDiagram(_tables);
            }
            //System.Threading.Thread.Sleep(10000);
          
            var ws = GetData.GetService();
            ws.GetRelationUsingDataContractCompleted += rel_GetRelationUsingDataContractCompleted;
            ws.GetRelationUsingDataContractAsync(guids);
            //this.Progress.Stop();
        }

        private void UpdateDiagram(ObservableCollection<DataTableInfo> _tables)
         {           
            List<EntityEx> obcollection = new List<EntityEx>();           
            foreach (var table in _tables)
            {
                DataTableInfo dtInfo = table as DataTableInfo;
                if (dtInfo != null)
                {
                    List<string> subNodes = dtInfo.Rows.Select(di => di.DataRow["ClassName"].ToString()).Distinct().ToList<string>();
                    if (subNodes.Count() > 0)
                    {                        
                        obcollection.Add(new EntityEx() 
                        {
                            Key = dtInfo.TableName,
                            RootName = String.Empty,
                            Items = subNodes.Select(clsName => new Attribute(clsName.ToString(), false, NodeFigure.Decision, "Yellow")).ToArray()
                        });

                        foreach (string clsname in subNodes)
                        {
                            obcollection.Add(new EntityEx()
                            {
                                Key = clsname,
                                RootName = dtInfo.TableName,
                                Items = dtInfo.Rows.Where(di => di.DataRow["ClassName"].ToString().Equals(clsname)).Select(c => new Attribute(c.DataRow["AttributeName"].ToString(), true, NodeFigure.Decision, "Yellow")).ToArray()                                
                            });
                        }
                    }                   
                }
            }

            MainDataCollection = obcollection;       
           
        }
    }

    public class EntityEx : GraphLinksModelNodeData<String>
    {
        // If the node data is copied, we need to override this method to make sure the list of items is not shared
        public override object Clone()
        {
            Entity d = (Entity)base.Clone();
            List<Attribute> items = new List<Attribute>();
            foreach (Attribute e in this.Items) items.Add(new Attribute(e.Name, e.IsKey, e.Figure, e.Color));
            d.Items = items;
            return d;
        }

        public IList<Attribute> Items
        {
            get { return _Items; }
            set
            {
                if (_Items != value)
                {
                    System.Collections.IEnumerable old = _Items;
                    _Items = value;
                    RaisePropertyChanged("Items", old, value);
                }
            }
        }
        private IList<Attribute> _Items = new List<Attribute>().AsReadOnly();

        //root name
        public String RootName
        {
            get { return _RootName; }
            set
            {
                if (_RootName != value)
                {
                    String old = _RootName;
                    _RootName = value;
                    RaisePropertyChanged("RootName", old, value);
                }
            }
        }
        private String _RootName = String.Empty;

        public bool EverExpanded { get; set; }

        public String Color { get; set; }

    }
      
}
