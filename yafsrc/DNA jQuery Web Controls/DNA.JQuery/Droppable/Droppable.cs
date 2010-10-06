﻿///  Copyright (c) 2009 Ray Liang (http://www.dotnetage.com)
///  Dual licensed under the MIT and GPL licenses:
///  http://www.opensource.org/licenses/mit-license.php
///  http://www.gnu.org/licenses/gpl.html
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Permissions;
using System.ComponentModel;
using System.Web.UI.Design;
using System.Drawing.Design;
using System.ComponentModel.Design;

namespace DNA.UI.JQuery
{
    /// <summary>
    /// 	<para>Droppable is a ASP.NET WebControl that encapsulat the jQuery UI droppable
    ///     plugin.Droppable Web Control implement all functions of jQuery UI droppable
    ///     plugin,it makes droppable plugin not only apply to client elements but also APS.NET
    ///     WebControls.That is a easy way to using jQuery UI droppable plugin on
    ///     ASP.NET.</para>
    /// 	<para>Droppable WebControl makes selected WebControls droppable (meaning they
    ///     accept being dropped on by draggables).</para>
    /// </summary>
    /// <example>
    ///     The control above is initialized with this code
    ///     <code lang="ASP.NET" title="Droppable properties of ASP.NET">
    /// 		<![CDATA[
    /// <DotNetAge:Droppable
    ///      ID="MyDroppable" 
    ///      runat="Server"
    ///      ActiveCssClass="ui-state-active"
    ///      HoverCssClass="ui-state-highlight"
    ///      Cursor="pointer"
    ///      CursorAt="left:50"
    ///      Container="Parent"
    ///      AllowAddClasses="True"
    ///      Greedy="Flase"
    ///      AutoScroll="True"
    ///      ScrollSpeed="100"
    ///      ScrollSensitivity="50"
    ///      DragHelperOpacity="0.5"
    ///      ZIndex="1000"
    ///      SnapX="50"
    ///      SnapY="50"
    ///      DragGroupName="dropGroup"
    ///      Tolerance="Intersect"
    ///      DragStartDelay="1000"
    ///      DragStartDistance="50"
    ///      DragHelper="Clone"
    ///      Orientation="Both"
    ///      OnClientDragActive="javascript/jQuery script like this:$(this).val();do sth ..."
    ///      OnClientDragDeactive="..." 
    ///      OnClientDragOver="..."
    ///      OnClientDragOut="..."
    ///      OnClientDrop="..."
    ///  >
    ///       <Target Selector="jQuery selector 
    ///                TargetID="ServerControlID"
    ///                TargetIDs="ControlID1,...,ControlIDn" />
    ///       <Accept ... />
    ///      <Containsin ... />
    ///      <DragHandler ... />
    ///      <DisableDraggingElements ... />
    /// <DotNetAge:Droppable>
    /// ]]>
    /// 	</code>
    /// </example>
    /// <remarks>
    ///     You can specify which (individually) or which kind of draggables each will accept.
    ///     All ClientEvents (OnClient... properties)receive two arguments(it's same of jQuery
    ///     droppable callbacks):
    ///     <para>The original browser event and a prepared ui object, view below for a
    ///     documentation of this object (if you name your second argument 'ui'):</para>
    /// 	<para class="xmldocbulletlist"></para>
    /// 	<list type="bullet">
    /// 		<item>ui.draggable - current draggable element, a jQuery object.</item>
    /// 		<item>ui.helper - current draggable helper, a jQuery object</item>
    /// 		<item>ui.position - current position of the draggable helper { top: , left: }</item>
    /// 		<item>ui.offset - current absolute position of the draggable helper { top: ,left: }</item>
    /// 	</list>
    /// </remarks>
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:Droppable runat=\"server\" ID=\"Droppable1\"></{0}:Droppable>")]
    [Designer(typeof(Design.NoneUIControlDesigner))]
    [JQuery(Name = "droppable", Assembly = "jQueryNet", ScriptResources = new string[] { "ui.core.js","ui.droppable.js" },
        StartEvent = ClientRegisterEvents.ApplicationLoad)]
    [ParseChildren(true)]
    [System.Drawing.ToolboxBitmap(typeof(Droppable), "Droppable.Droppable.ico")]
    public class Droppable : Control,INamingContainer
    {
        private JQuerySelector accept;
        private JQuerySelector target = new JQuerySelector();

        /// <summary>
        /// Apply the jQuery droppable plugin to the target control
        /// </summary>
        /// <param name="control">the instance of the target control</param>
        /// <returns>the Droppable instance which have been created will return.</returns>
        public static Droppable ApplyTo(Control control)
        {
            Droppable droppable = new Droppable();
            droppable.Target = new JQuerySelector(control);
            control.Page.Controls.Add(droppable);
            return droppable;
        }

        /// <summary>
        /// Apply the jQuery droppable plugin to the target controls
        /// </summary>
        /// <param name="control">the instances of the target controls</param>
        /// <returns>the Droppable instance which have been created will return.</returns>
        public static Droppable ApplyTo(params Control[] control)
        {
            Droppable droppable = new Droppable();
            string[] ids = new string[control.Length];
            for (int i = 0; i < control.Length; i++)
                ids[i] = "#" + control[i].ClientID;
            droppable.Target = new JQuerySelector(ids);
            control[0].Page.Controls.Add(droppable);
            return droppable;
        }

        /// <summary>
        ///  Apply the jQuery droppable plugin to the specify WebControls/HtmlElements by jQuery server side selctor
        /// </summary>
        /// <param name="page">the Page instance that contains the WebControls/HtmlElements</param>
        /// <param name="selector"> jQuery server side selctor instance</param>
        /// <returns>the Droppable instance which have been created will return.</returns>
        public static Droppable ApplyTo(Page page, JQuerySelector selector)
        {
            Droppable droppable = new Droppable();
            droppable.Target = selector;
            page.Controls.Add(droppable);
            return droppable;
        }

        /// <summary>
        /// Gets/Sets which control to apply droppable
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [Category("Behavior")]
        [Description("Gets/Sets which control to apply droppable")]
        public JQuerySelector Target
        {
            get { return target; }
            set { target = value; }
        }


        /// <summary>
        /// Gets/Sets all draggables that match the selector will be accepted. 
        /// </summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [NotifyParentProperty(true)]
        [JQueryOption("accept")]
        [Category("Behavior")]
        [Description("Gets/Sets all draggables that match the selector will be accepted. ")]
        public JQuerySelector Accept 
        {
            get 
            {
                if (accept == null)
                {
                    accept = new JQuerySelector();
                    accept.ExpressionOnly = true;
                }
                return accept; 
            }
            set 
            { 
                accept = value;
                if (value != null)
                    accept.ExpressionOnly = true;
            }
        }

        /// <summary>
        /// Gets/Sets the class will be added to the droppable while an acceptable draggable is being dragged.
        /// </summary>
        [JQueryOption("activeClass")]
        [CssClassProperty]
        [Themeable(true)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets/Sets the class will be added to the droppable while an acceptable draggable is being dragged.")]
        public string ActiveCssClass
        {
            get
            {
                Object obj = ViewState["ActiveCssClass"];
                return (obj == null) ? null: (string)obj;
            }
            set
            {
                ViewState["ActiveCssClass"] = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether prevent the ui-droppable class from being added. 
        /// </summary>
        /// <remarks>
        /// This may be desired as a performance optimization when calling .droppable() init on many hundreds of elements.
        /// </remarks>
        [JQueryOption("addClasses",IgnoreValue=true)]
        [Themeable(true)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets/Sets whether prevent the ui-droppable class from being added. ")]
        public bool AllowAddClasses
        {
            get
            {
                Object obj = ViewState["AddClasses"];
                return (obj == null) ? true : (bool)obj;
            }
            set
            {
                ViewState["AddClasses"] = value;
            }
        }

        /// <summary>
        /// Gets/Sets whether prevent event propagation on nested droppables.
        /// </summary>
        [JQueryOption("greedy",IgnoreValue=false)]
        public bool Greedy
        {
            get
            {
                Object obj = ViewState["Greedy"];
                return (obj == null) ? false : (bool)obj;
            }
            set
            {
                ViewState["Greedy"] = value;
            }
        }

        /// <summary>
        /// Gets/Sets the class will be added to the droppable while an acceptable draggable is being hovered.
        /// </summary>
        [JQueryOption("hoverClass")]
        [CssClassProperty]
        [Themeable(true)]
        [Bindable(true)]
        [Category("Appearance")]
        [Description("Gets/Sets the class will be added to the droppable while an acceptable draggable is being hovered.")]
        public string HoverCssClass
        {
            get
            {
                Object obj = ViewState["HoverCssClass"];
                return (obj == null) ? String.Empty : (string)obj;
            }
            set
            {
                ViewState["HoverCssClass"] = value;
            }
        }

        /// <summary>
        /// Gets/Sets the group sets of draggable and droppable items, in addition to droppable's Accept Property. 
        /// </summary>
        [JQueryOption("scope")]
        [Category("Behavior")]
        [Description("Gets/Sets the group sets of draggable and droppable items, in addition to droppable's Accept Property. ")]
        public string DragGroupName
        {
            get
            {
                Object obj = ViewState["DragGroupName"];
                return (obj == null) ? null : (string)obj;
            }
            set
            {
                ViewState["DragGroupName"] = value;
            }
        }

        /// <summary>
        /// Gets/Sets which mode to use for testing whether a draggable is 'over' a droppable. Possible values: 'fit', 'intersect', 'pointer', 'touch'.
        /// </summary>
        [JQueryOption("tolerance",IgnoreValue=DroppableTolerances.Intersect)]
        [Category("Behavior")]
        [Description("Gets/Sets which mode to use for testing whether a draggable is 'over' a droppable. Possible values: 'fit', 'intersect', 'pointer', 'touch'.")]
        public DroppableTolerances Tolerance
        {
            get
            {
                Object obj = ViewState["Tolerance"];
                return (obj == null) ? DroppableTolerances.Intersect : (DroppableTolerances)obj;
            }
            set
            {
                ViewState["Tolerance"] = value;
            }
        }

        #region ClientEvents
        /// <summary>
        /// This event is triggered any time an accepted draggable starts dragging. This can be useful if you want to make the droppable 'light up' when it can be dropped on
        /// </summary>
        [Category("ClientEvents")]
        [Description("This event is triggered any time an accepted draggable starts dragging. This can be useful if you want to make the droppable 'light up' when it can be dropped on")]
        [TypeConverter(typeof(MultilineStringConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Bindable(true)]
        [JQueryOption("activate", Type = JQueryOptionTypes.Function, FunctionParams = new string[] { "event", "ui" })]
        public string OnClientDragActive { get; set; }

        /// <summary>
        /// This event is triggered any time an accepted draggable stops dragging
        /// </summary>
        [Category("ClientEvents")]
        [Description("This event is triggered any time an accepted draggable stops dragging")]
        [TypeConverter(typeof(MultilineStringConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Bindable(true)]
        [JQueryOption("deactivate", Type = JQueryOptionTypes.Function, FunctionParams = new string[] { "event", "ui" })]
        public string OnClientDragDeactive { get; set; }

        /// <summary>
        /// This event is triggered as an accepted draggable is dragged 'over' (within the tolerance of) this droppable
        /// </summary>
        [Category("ClientEvents")]
        [Description("This event is triggered as an accepted draggable is dragged 'over' (within the tolerance of) this droppable")]
        [TypeConverter(typeof(MultilineStringConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Bindable(true)]
        [JQueryOption("over", Type = JQueryOptionTypes.Function, FunctionParams = new string[] { "event", "ui" })]
        public string OnClientDragOver { get; set; }

        /// <summary>
        /// This event is triggered when an accepted draggable is dragged out (within the tolerance of) this droppable
        /// </summary>
        [Category("ClientEvents")]
        [Description("This event is triggered when an accepted draggable is dragged out (within the tolerance of) this droppable")]
        [TypeConverter(typeof(MultilineStringConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Bindable(true)]
        [JQueryOption("out", Type = JQueryOptionTypes.Function, FunctionParams = new string[] { "event", "ui" })]
        public string OnClientDragOut { get; set; }

        /// <summary>
        /// This event is triggered when an accepted draggable is dropped 'over'
        /// (within the tolerance of) this droppable. In the callback, $(this) represents 
        /// the droppable the draggable is dropped on. ui.draggable represents the draggable
        /// </summary>
        [Category("ClientEvents")]
        [Description("")]
        [TypeConverter(typeof(MultilineStringConverter))]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        [Bindable(true)]
        [JQueryOption("drop", Type = JQueryOptionTypes.Function, FunctionParams = new string[] { "event", "ui" })]
        public string OnClientDrop { get; set; }
        #endregion
        
        //protected override void OnInit(EventArgs e)
        //{
        //    Page.RegisterRequiresPostBack(this);
        //    Page.ClientScript.RegisterHiddenField(HiddenKey, "");
        //    base.OnInit(e);
        //}

        //private string HiddenKey
        //{
        //    get { return this.ClientID + "_hidden"; }
        //}

        protected override void OnPreRender(EventArgs e)
        {
            JQueryScriptBuilder builder = new JQueryScriptBuilder(this,Target);
            //builder.Prepare();
            //builder.Build();
            //ScriptBuilder script = new ScriptBuilder();
            //script.AppendSetValue(HiddenKey, "'{id:event.target.id,css:event.target.style.cssText}");
            //builder.AppendBindFunction("drag", new string[] { "event", "ui" }, script.ToString());
            ClientScriptManager.RegisterJQueryControl(this,builder);
            base.OnPreRender(e);
        }
    }
}
