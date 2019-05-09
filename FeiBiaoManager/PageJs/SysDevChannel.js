layui.use(['jquery', 'layer', 'element', 'laypage', 'laytpl'], function () {
    window.jQuery = window.$ = layui.jquery;
    window.layer = layui.layer;
    var element = layui.element(),
        laypage = layui.laypage,
        laytpl = layui.laytpl;


    var index = parent.layer.getFrameIndex(window.name);//获取父窗体
    var KeyId = parent.$("#KeyId").val();//获取修改Id，隐藏域
    var parms = { action: "GetSysDevChannel", IVASId: KeyId };

    /*
    * 加载方法
    */
    $.post(SysConfig.API.GetDeviceData, parms, function (data) {
        var getTpl = $("#demo").html();
        laytpl(getTpl).render(data, function (html) {
            $("#listdata").html(html);
        });
    }, "json");
});