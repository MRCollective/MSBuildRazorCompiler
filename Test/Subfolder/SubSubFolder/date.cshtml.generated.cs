﻿#pragma checksum "Subfolder\SubSubFolder\date.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7ba6650872f797b9e9c58f4ec23ec716e128b62b"
// <auto-generated/>
#pragma warning disable 1591
[assembly:global::RazorLight.Razor.RazorLightTemplateAttribute(@"Subfolder\SubSubFolder\date.cshtml", typeof(Test.Subfolder.SubSubFolder.Date))]
namespace Test.Subfolder.SubSubFolder
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
#nullable restore
#line 1 "Subfolder\SubSubFolder\date.cshtml"
using System;

#line default
#line hidden
#nullable disable
    public class Date : RazorLight.TemplatePage<object>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<p>");
#nullable restore
#line 3 "Subfolder\SubSubFolder\date.cshtml"
Write(DateTime.Now);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591