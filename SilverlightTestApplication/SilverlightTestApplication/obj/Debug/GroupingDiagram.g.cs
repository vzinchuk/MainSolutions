﻿#pragma checksum "C:\Users\vzinchuk\Desktop\ProjectSimple\SilverlightTestApplication\SilverlightTestApplication\GroupingDiagram.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1A05C61757567D86D4CEB37EFF25E81C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace SilverlightTestApplication {
    
    
    public partial class GroupingDiagram : System.Windows.Controls.UserControl {
        
        internal Northwoods.GoXam.Diagram myDiagram;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/SilverlightTestApplication;component/GroupingDiagram.xaml", System.UriKind.Relative));
            this.myDiagram = ((Northwoods.GoXam.Diagram)(this.FindName("myDiagram")));
        }
    }
}

