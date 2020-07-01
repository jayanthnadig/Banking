using Newtonsoft.Json;
using System.Globalization;
using System.Threading.Tasks;
using ASNRTech.CoreService.Core;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Logging;

namespace ASNRTech.CoreService.Services {
  public class EventService : BaseService {

    internal static async Task LogAsync(TeamHttpContext teamContext, TeamDbContext dbContext, string parentType, string parentId, string name) {
      await LogAsync(teamContext, dbContext, parentType, parentId, name, null).ConfigureAwait(false);
    }

    internal static async Task LogAsync(TeamHttpContext teamContext, TeamDbContext dbContext, string parentType, int parentId, string name) {
      await LogAsync(teamContext, dbContext, parentType, parentId.ToString(CultureInfo.InvariantCulture), name, null).ConfigureAwait(false);
    }

    internal static async Task LogAsync(TeamHttpContext teamContext, string parentType, string parentId, string name) {
      using (TeamDbContext dbContext = new TeamDbContext()) {
        await LogAsync(teamContext, dbContext, parentType, parentId, name, null).ConfigureAwait(false);
      }
    }

    internal static async Task LogAsync(TeamHttpContext teamContext, string parentType, int parentId, string name) {
      using (TeamDbContext dbContext = new TeamDbContext()) {
        await LogAsync(teamContext, dbContext, parentType, parentId.ToString(CultureInfo.InvariantCulture), name, null).ConfigureAwait(false);
      }
    }

    internal static async Task LogAsync(TeamHttpContext teamContext, TeamDbContext dbContext, string parentType, string parentId, string name, object data) {
      dbContext.Events.Add(new AppEvent {
        ParentType = parentType,
        ParentId = parentId,
        Name = name,
        Data = JsonConvert.SerializeObject(data)
      });
      await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
  }
}
