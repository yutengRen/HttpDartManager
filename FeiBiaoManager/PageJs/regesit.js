layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl', 'form'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl,
        form = layui.form();

    //监听提交
    form.on('submit(setmyinfo)', function (data) {
        var parms = data.field;
        parms.action = "regesit";
        $.post(SysConfig.API.managerDate, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    layer.msg(data.Msg);
                }
                else {
                    layer.msg(data.Msg);
                }
            }
            else {
                layer.msg("添加失败：数据为空");
            }
        }, "json");
        return false;
    });
});