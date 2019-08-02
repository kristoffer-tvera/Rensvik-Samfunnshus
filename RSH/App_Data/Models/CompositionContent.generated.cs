//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.10.102
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace Umbraco.Web.PublishedContentModels
{
	// Mixin content Type 1058 with alias "compositionContent"
	/// <summary>Composition - Content</summary>
	public partial interface ICompositionContent : IPublishedContent
	{
		/// <summary>Banner</summary>
		IEnumerable<IPublishedContent> Banner { get; }

		/// <summary>Description</summary>
		string Description { get; }

		/// <summary>Grid</summary>
		Newtonsoft.Json.Linq.JToken Grid { get; }
	}

	/// <summary>Composition - Content</summary>
	[PublishedContentModel("compositionContent")]
	public partial class CompositionContent : PublishedContentModel, ICompositionContent
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "compositionContent";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public CompositionContent(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<CompositionContent, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Banner
		///</summary>
		[ImplementPropertyType("banner")]
		public IEnumerable<IPublishedContent> Banner
		{
			get { return GetBanner(this); }
		}

		/// <summary>Static getter for Banner</summary>
		public static IEnumerable<IPublishedContent> GetBanner(ICompositionContent that) { return that.GetPropertyValue<IEnumerable<IPublishedContent>>("banner"); }

		///<summary>
		/// Description
		///</summary>
		[ImplementPropertyType("description")]
		public string Description
		{
			get { return GetDescription(this); }
		}

		/// <summary>Static getter for Description</summary>
		public static string GetDescription(ICompositionContent that) { return that.GetPropertyValue<string>("description"); }

		///<summary>
		/// Grid
		///</summary>
		[ImplementPropertyType("grid")]
		public Newtonsoft.Json.Linq.JToken Grid
		{
			get { return GetGrid(this); }
		}

		/// <summary>Static getter for Grid</summary>
		public static Newtonsoft.Json.Linq.JToken GetGrid(ICompositionContent that) { return that.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("grid"); }
	}
}
