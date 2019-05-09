layui.use(['jquery', 'form', 'upload'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var form = layui.form();

    var index = parent.layer.getFrameIndex(window.name);//获取父窗体
    var KeyId = parent.$("#KeyId").val();//获取修改Id，隐藏域

    //监听提交
    form.on('submit(btn_sub)', function (data) {
        //编辑，包含新增修改
        var parms = { action: "Edit", UserId: KeyId };
        ComMethod.AddGroupJson(parms, data.field);

        if (parms == null) return;
        $.post(SysConfig.API.GetGoodTypeData, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    parent.layer.close(index);//关闭窗口                             
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
        }, "json");

        return false;
    });


    //编辑时查询显示表单
    if (KeyId) {
        var parms = { action: "View", UserId: KeyId };

        $.post(SysConfig.API.GetGoodTypeData, parms, function (data) {
            if (data != null) {
                if (data.Success) {
                    //显示表单值
                    ComMethod.SetFormData("form1", data.Result);
                    form.render();//更新全部表单
                }
                else {
                    layer.msg("添加失败：" + data.Msg);
                }
            }
        }, "json");
    }
});