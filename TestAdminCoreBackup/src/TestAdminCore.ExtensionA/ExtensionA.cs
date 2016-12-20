// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using ExtCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace TestAdminCore.ExtensionA
{
  public class ExtensionA : ExtensionBase
  {
    public override IEnumerable<KeyValuePair<int, Action<IRouteBuilder>>> UseMvcActionsByPriorities
    {
      get
      {
        return new Dictionary<int, Action<IRouteBuilder>>()
        {
          [1000] = routeBuilder =>
          {
              routeBuilder.MapRoute(name: "A Index", template: "Extensions/" + "userrole", defaults: new { controller = "ExtensionA", action = "Index" });
              routeBuilder.MapRoute(name: "A subpages", template: "Extensions/" + "userrole/{action}", defaults: new { controller = "ExtensionA", action = "{action}" });
          }
        };
      }
    }
  }
}