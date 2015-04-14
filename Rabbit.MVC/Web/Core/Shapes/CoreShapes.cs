using Rabbit.Kernel.Localization;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.Web.Mvc.DisplayManagement;
using Rabbit.Web.Mvc.DisplayManagement.Descriptors;
using Rabbit.Web.Mvc.UI;
using Rabbit.Web.Mvc.UI.Navigation;
using Rabbit.Web.Mvc.UI.Zones;
using Rabbit.Web.UI.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rabbit.Core.Shapes
{
    internal sealed class CoreShapes : IShapeTableProvider
    {
        #region Field

        private readonly IShapeFactory _shapeFactory;
        private readonly INavigationManager _navigationManager;

        #endregion Field

        #region Constructor

        public CoreShapes(IShapeFactory shapeFactory, INavigationManager navigationManager)
        {
            _shapeFactory = shapeFactory;
            _navigationManager = navigationManager;

            T = NullLocalizer.Instance;
        }

        #endregion Constructor

        #region Property

        public Localizer T { get; set; }

        public dynamic New { get { return _shapeFactory; } }

        #endregion Property

        #region Implementation of IShapeTableProvider

        /// <summary>
        /// 发现形状。
        /// </summary>
        /// <param name="builder">形状表格建造者。</param>
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("Layout")
                .Configure(descriptor => descriptor.Wrappers.Add("Document"))
                .OnCreating(creating => creating.Create = () => new ZoneHolding(() => creating.New.Zone()))
                .OnCreated(created =>
                {
                    var layout = created.Shape;

                    layout.Head = created.New.DocumentZone(ZoneName: "Head");
                    layout.Body = created.New.DocumentZone(ZoneName: "Body");
                    layout.Tail = created.New.DocumentZone(ZoneName: "Tail");

                    layout.Body.Add(created.New.PlaceChildContent(Source: layout));

                    layout.Content = created.New.Zone();
                    layout.Content.ZoneName = "Content";
                    layout.Content.Add(created.New.PlaceChildContent(Source: layout));

                    layout.Breadcrumb = created.New.Zone();
                    layout.Breadcrumb.ZoneName = "Breadcrumb";
                    layout.Breadcrumb.Add(created.New.Breadcrumb(GetMenus: new Func<HttpRequestBase, RouteValueDictionary, IEnumerable<MenuItem>>((request, routeValues) => NavigationHelper.SetSelectedPath(_navigationManager.BuildMenu("admin"), request, routeValues))));

                    layout.User = created.New.User();
                });

            builder.Describe("Zone")
                .OnCreating(creating => creating.Create = () => new Zone())
                .OnDisplaying(displaying =>
                {
                    var zone = displaying.Shape;
                    string zoneName = zone.ZoneName;
                    zone.Classes.Add("zone-" + HtmlClassify(zoneName));
                    zone.Classes.Add("zone-" + zoneName);
                    zone.Classes.Add("zone");

                    zone.Metadata.Alternates.Add("Zone__" + zoneName);
                });

            builder.Describe("Menu")
                .OnDisplaying(displaying =>
                {
                    var menu = displaying.Shape;
                    string menuName = menu.MenuName;
                    menu.Classes.Add("menu-" + HtmlClassify(menuName));
                    menu.Classes.Add("menu");
                    menu.Classes.Add("nav");
                    menu.Classes.Add("nav-list");
                    menu.Metadata.Alternates.Add("Menu__" + EncodeAlternateElement(menuName));
                });

            builder.Describe("MenuItem")
                .OnDisplaying(displaying =>
                {
                    var menuItem = displaying.Shape;
                    var menu = menuItem.Menu;
                    menuItem.Metadata.Alternates.Add("MenuItem__" + EncodeAlternateElement(menu.MenuName));
                });

            builder.Describe("Breadcrumb")
                .OnDisplaying(displaying =>
                {
                    var shape = displaying.Shape;
                    var metadata = shape.Metadata;
                    metadata.Alternates.Add("Breadcrumb");
                });
        }

        #endregion Implementation of IShapeTableProvider

        #region Shapes

        [Shape]
        public void Zone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            string id = Shape.Id;
            IEnumerable<string> classes = Shape.Classes;
            IDictionary<string, string> attributes = Shape.Attributes;
            var zoneWrapper = GetTagBuilder("div", id, classes, attributes);
            Output.Write(zoneWrapper.ToString(TagRenderMode.StartTag));
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
            Output.Write(zoneWrapper.ToString(TagRenderMode.EndTag));
        }

        [Shape]
        public void ContentZone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
        }

        [Shape]
        public void DocumentZone(dynamic Display, dynamic Shape, TextWriter Output)
        {
            foreach (var item in ordered_hack(Shape))
                Output.Write(Display(item));
        }

        [Shape]
        public IHtmlString PlaceChildContent(dynamic Source)
        {
            return Source.Metadata.ChildContent;
        }

        #endregion Shapes

        #region Private Method

        #region Ordered Hack

        private static IEnumerable<dynamic> ordered_hack(dynamic shape)
        {
            IEnumerable<dynamic> unordered = shape;
            if (unordered == null || unordered.Count() < 2)
                return shape;

            var i = 1;
            var progress = 1;
            var flatPositionComparer = new FlatPositionComparer();
            var ordering = unordered.Select(item =>
            {
                var position = (item == null || item.GetType().GetProperty("Metadata") == null || item.Metadata.GetType().GetProperty("Position") == null)
                                   ? null
                                   : item.Metadata.Position;
                return new { item, position };
            }).ToList();

            while (i < ordering.Count())
            {
                if (flatPositionComparer.Compare(ordering[i].position, ordering[i - 1].position) > -1)
                {
                    if (i == progress)
                        progress = ++i;
                    else
                        i = progress;
                }
                else
                {
                    var higherThanItShouldBe = ordering[i];
                    ordering[i] = ordering[i - 1];
                    ordering[i - 1] = higherThanItShouldBe;
                    if (i > 1)
                        --i;
                }
            }

            return ordering.Select(ordered => ordered.item).ToList();
        }

        #endregion Ordered Hack

        private static TagBuilder GetTagBuilder(string tagName, string id, IEnumerable<string> classes, IDictionary<string, string> attributes)
        {
            var tagBuilder = new TagBuilder(tagName);
            tagBuilder.MergeAttributes(attributes, false);
            foreach (var cssClass in classes ?? Enumerable.Empty<string>())
                tagBuilder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(id))
                tagBuilder.GenerateId(id);
            return tagBuilder;
        }

        private static string EncodeAlternateElement(string alternateElement)
        {
            return alternateElement.Replace("-", "__").Replace(".", "_");
        }

        /// <summary>
        /// Html样式类名称。
        /// </summary>
        /// <param name="text">字符串。</param>
        /// <returns>处理后的字符串。</returns>
        private static string HtmlClassify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var friendlier = text.CamelFriendly();

            var result = new char[friendlier.Length];

            var cursor = 0;
            var previousIsNotLetter = false;
            for (var i = 0; i < friendlier.Length; i++)
            {
                var current = friendlier[i];
                if (current.IsLetter() || (char.IsDigit(current) && cursor > 0))
                {
                    if (previousIsNotLetter && i != 0 && cursor > 0)
                    {
                        result[cursor++] = '-';
                    }

                    result[cursor++] = char.ToLowerInvariant(current);
                    previousIsNotLetter = false;
                }
                else
                {
                    previousIsNotLetter = true;
                }
            }

            return new string(result, 0, cursor);
        }

        #endregion Private Method
    }
}