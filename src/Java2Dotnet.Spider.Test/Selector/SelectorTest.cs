﻿using Java2Dotnet.Spider.Core;
using Java2Dotnet.Spider.Core.Selector;
#if !NET_CORE
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace Java2Dotnet.Spider.Test.Selector
{
	[TestClass]
	public class SelectorTest
	{
		private string _html = "<div><a href='http://whatever.com/aaa'></a></div><div><a href='http://whatever.com/bbb'></a></div>";

		[TestMethod]
		public void TestChain()
		{
			Selectable selectable = new Selectable(_html, "", ContentType.Html);
			var linksWithoutChain = selectable.Links().GetValue();
			ISelectable xpath = selectable.XPath("//div");
			var linksWithChainFirstCall = xpath.Links().GetValue();
			var linksWithChainSecondCall = xpath.Links().GetValue();
			Assert.AreEqual(linksWithoutChain.Count, linksWithChainFirstCall.Count);
			Assert.AreEqual(linksWithChainFirstCall.Count, linksWithChainSecondCall.Count);
		}

		[TestMethod]
		public void TestNodes()
		{
			Selectable selectable = new Selectable(_html, "", ContentType.Html);
			var links = selectable.XPath(".//a/@href").Nodes();
			Assert.AreEqual(links[0].GetValue(), "http://whatever.com/aaa");

			var links1 = selectable.XPath(".//a/@href").GetValue();
			Assert.AreEqual(links1[0], "http://whatever.com/aaa");
		}
	}
}
