using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace ASNRTech.CoreService.Security {
  public class AddAuthorizeFilterConvention : IControllerModelConvention {

    public void Apply(ControllerModel controller) {
      if (controller == null) {
        throw new System.ArgumentNullException(nameof(controller));
      }
      //if (!controller.Filters.Contains(typeof(TeamAuthorizeAttribute))) {
      //  controller.Filters.Add(new TeamAuthorizeAttribute(AccessType.None, false));
      //}
      IList<IFilterMetadata> filters = controller.Filters;
      int count = filters.Count;
    }
  }
}
