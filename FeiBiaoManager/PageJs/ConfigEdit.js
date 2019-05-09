layui.use(['jquery', 'form', 'layedit'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var form = layui.form(),
        layedit = layui.layedit;

    var index = parent.layer.getFrameIndex(window.name);//获取父窗体


    //关于我们提交
    form.on('submit(btn_sub)', function (data) {

        var parms = { action: "SetConfig", PId: $("#KeyId").val() };
        ComMethod.AddGroupJson(parms, data.field);
        if (parms == null) return;
        $.post(SysConfig.API.GetInfoData, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    layer.msg("保存成功");
                }
                else {
                    layer.msg("保存失败：" + data.Msg);
                }
            }
        }, "json");

        return false;
    });



    var InitData = function () {
        var parms = { action: "ViewConfig", PId: $("#KeyId").val() };

        if (parms == null) return;
        $.post(SysConfig.API.GetInfoData, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    //显示表单值
                    ComMethod.SetFormData("form1", data.Result);
                }
            }
        }, "json");
    }

    InitData();

});