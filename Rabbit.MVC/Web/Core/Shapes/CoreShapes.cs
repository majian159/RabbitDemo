using Rabbit.Kernel.Localization;
using Rabbit.Kernel.Works;
using Rabbit.Web.Mvc.DisplayManagement;
using Rabbit.Web.Mvc.DisplayManagement.Descriptors;
using Rabbit.Web.Mvc.UI;
using Rabbit.Web.Mvc.UI.Zones;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rabbit.Core.Shapes
{
    internal sealed class CoreShapes : IShapeTableProvider
    {
        #region Field

        private readonly Work<IShapeFactory> _shapeFactory;

        #endregion Field

        #region Constructor

        public CoreShapes(Work<IShapeFactory> shapeFactory)
        {
            _shapeFactory = shapeFactory;

            T = NullLocalizer.Instance;
        }

        #endregion Constructor

        #region Property

        public Localizer T { get; set; }

        public dynamic New { get { return _shapeFactory.Value; } }

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
                });

            builder.Describe("Zone")
                .OnCreating(creating => creating.Create = () => new Zone())
                .OnDisplaying(displaying =>
                {
                    var zone = displaying.Shape;
                    string zoneName = zone.ZoneName;
                    //                    zone.Classes.Add("zone-" + zoneName.HtmlClassify());
                    zone.Classes.Add("zone-" + zoneName);
                    zone.Classes.Add("zone");

                    zone.Metadata.Alternates.Add("Zone__" + zoneName);
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

        #endregion Private Method
    }
}