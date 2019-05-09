/**
*公共方法类
*/
var ComMethod = {

    /**
    * 获取表单提交数据值，包含条件分页
    * @param fid 表单区域id
    * @param action 操作权限
    */
    GetFormData: function (fid, action) {
        var postdata = [];
        postdata.push("{\"action\":\"" + action + "\"");
        $("#" + fid + " input[type!='checkbox'],select").each(function () {
            if ($(this).prop("type") != "radio")
                postdata.push("\"" + $(this).prop("name") + "\":\'" + $(this).val() + "\'");
        });
        $("#" + fid + " textarea").each(function () {
            postdata.push("\"" + $(this).prop("name") + "\":\'" + escape($(this).val()) + "\'");
        });

        var ckName = [], ckVal = [];
        $("#" + fid + " input[type='checkbox']").each(function () {
            var thisName = $(this).prop("name");
            if ($.inArray(thisName, ckName) == -1) {
                ckName.push(thisName);
            }
        });
        for (var i = 0; i < ckName.length; i++) {
            $("#" + fid + " input[name='" + ckName[i] + "']").each(function () {
                if ($(this).prop("checked")) {
                    //ckVal.push($(this).val());
                    postdata.push("\"" + ckName[i] + "\":\'" + $(this).val() + "\'");
                }
            });

        }

        ckName = [], ckVal = [];
        $("#" + fid + " input[type='radio']").each(function () {
            var thisName = $(this).prop("name");
            if ($.inArray(thisName, ckName) == -1) {
                ckName.push(thisName);
            }
        });
        for (var i = 0; i < ckName.length; i++) {
            $("#" + fid + " input[name='" + ckName[i] + "']").each(function (i) {
                if ($(this).prop("checked")) {
                    ckVal.push($(this).val());
                }
            });
            postdata.push("\"" + ckName[i] + "\":\'" + ckVal.join(',') + "\'");
            ckVal = [];
        }

        postdata.push("\"pid\":1");//页码，默认1
        postdata.push("\"psize\":10");//页条数,默认10

        postdata.push("}");
        return eval("(" + postdata.join(',') + ")");
    },
    /**
    * 设置常用表单数据值
    * @param fid 表单区域id
    * @param jsonData 数据
    */
    SetFormData: function (fid, jsonData) {
        $("#" + fid + " input[type!='checkbox'],select,textarea").each(function () {
            var value = jsonData[$(this).prop("name")];
            if ($(this).prop("type") != "radio")
                $(this).val(value);
        });

        $("#" + fid + " input[type='radio']").each(function (i) {
            var value = jsonData[$(this).prop("name")];
            if (value == $(this).val())
                $(this).prop("checked", "checked");
        });
        $("#" + fid + " input[type='checkbox']").each(function () {
            var value = jsonData[$(this).prop("name")];
            if (value == $(this).val())
                $(this).prop("checked", "checked");
        });
    },
    /**
    * 设置列表checkbox选择
    */
    SetCheckList: function () {
        $("#selected-all").on("click", function (data) {
            var ret = $(this).prop("checked");
            var child = $(this).parents('table').find('tbody input[type="checkbox"]');            
            child.each(function (index, item) {
                $(this).prop("checked", ret);
            });
        });      
    },
    /**
    * 获取列表选择项
    */
    CheckItems: function () {
        var ids = [];
        var cks = $("#listdata input[type='checkbox']:checked");
        cks.each(function (index) {
            ids.push($(this).val());
        });
        return ids.join(',');
    },
    /**
    * 清除列表选择项
    */
    ClearCheck: function () {
        $("#selected-all").prop("checked", "");
        var child = $("#selected-all").parents('table').find('tbody input[type="checkbox"]');
        child.each(function (index, item) {
            $(this).prop("checked", "");
        });
        $("#KeyId").val("");
    },
    /*
    * 将两个JSON对象组装到一个里面
    * @param 目标JSON
    * @param 被组装JSON
    */
    AddGroupJson: function (targetJson, packJson) {
        if (targetJson && packJson) {
            for (var p in packJson) {
                targetJson[p] = packJson[p];
            }
        }
    },
    /*
    * 根据条件查找JSON指定字段值
    * @param json 目标JSON
    * @param cval 条件值
    * @param filed 返回数据字段
    */
    FindToJson: function (json, cval, filed) {
        if (json) {
            for (var p in json) {
                for (var c in json[p]) {
                    if (json[p][c] == cval) {
                        return json[p][filed];
                    }
                }
            }
        }
        return "";
    }
}


//$(function () {
//    // 设置jQuery Ajax全局的参数
//    $.ajaxSetup({
//        async: true,
//        error: function (jqXHR, textStatus, errorThrown) {
//            switch (jqXHR.status) {
//                case (500):
//                    alert("服务器系统内部错误");
//                    break;
//                case (401):
//                    alert("未登录");
//                    break;
//                case (403):
//                    alert("无权限执行此操作");
//                    break;
//                case (408):
//                    alert("请求超时");
//                    break;
//                default:
//                    alert("网络异常");
//            }
//            ComMethod.LoadEnd();
//        }
//    });

//});
