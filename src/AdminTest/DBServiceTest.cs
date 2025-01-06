// ------------------------------------------------------------------------------
// 此代码版权声明为全文件覆盖，如有原作者特别声明，会在下方手动补充
// 此代码版权（除特别声明外的代码）归作者本人Diego所有
// 源代码使用协议遵循本仓库的开源协议及附加协议
// Gitee源代码仓库：https://gitee.com/diego2098/ThingsGateway
// Github源代码仓库：https://github.com/kimdiego2098/ThingsGateway
// 使用文档：https://thingsgateway.cn/
// QQ群：605534569
// ------------------------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using ThingsGateway.Admin.Application;
using ThingsGateway.AdminServer;

using Xunit;
namespace AdminTest;

public class UnitTest1 : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public UnitTest1(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    //[InlineData("/default")]
    public async Task TestDB()
    {
        var sysRoleService = _factory.Services.GetRequiredService<ISysRoleService>();
        var data = await sysRoleService.GetAllAsync();

        // 使用 Assert 来检查 List 的大小是否大于 0
        Assert.True(data.Count > 0, "The list should have more than 0 elements");
    }
}