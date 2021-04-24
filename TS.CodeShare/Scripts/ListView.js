
function generateTable(objPlaceHolder, jsonObject, columnNames, makeSortable, hideFooterCount, arraySelectedRow, makeRowEditable, editableFunction) {
    var tableId = objPlaceHolder.id + "table";
    var ModalPopupId = objPlaceHolder.id + "ModalPopup";

    var table = "<div class=\"ListView\"><table id=\"" + tableId + "\"><thead><tr>";

    eval("var " + objPlaceHolder.id + "ColumnNames = columnNames;");
    eval("var " + objPlaceHolder.id + "JsonData = jsonObject;");

    for (var i = 0; i < columnNames.length; i++) {
        switch (columnNames[i].type) {
            case "primarycheckbox":
                var checkClass = tableId + columnNames[i].name.replace(/ /g, "_") + "checkbox";
                table += "<th data-type=\"primarycheckbox\" class=\"ListViewCheckbox\"><input class=\"" + checkClass + "\" type=\"checkbox\" onclick=\"toggleSelect(this);\"/></th>";
                break;

            case "primaryradio":
                table += "<th data-type=\"primaryradio\">&nbsp;</th>"
                break;

            case "serialnumber":
                table += "<th data-type=\"serialnumber\" class=\"ListViewSerialNumber\">" + columnNames[i].name + "</th>";
                break;

            case "textarea":
                table += "<th data-type=\"textarea\">" + columnNames[i].name + "</th>";
                break;

            case "dropdown":
                table += "<th data-type=\"dropdown\" " + (columnNames[i].onformat == null ? "" : " onformat=\"" + columnNames[i].onformat + "\" ") + " data-options=\"" + (columnNames[i].options == null ? "" : escape(JSON.stringify(columnNames[i].options))) + "\">" + columnNames[i].name + "</th>";
                break;

            case "bgcolor":
            case "fgcolor":
            case "editable":
                break;


            default:
                var width = null;

                if (columnNames[i].width != null)
                    width = " style=\"width: " + columnNames[i].width + ";\"";

                if (columnNames[i].rename == null)
                    table += "<th" + (width == null ? "" : width) + ">" + columnNames[i].name + "</th>";
                else
                    table += "<th" + (width == null ? "" : width) + ">" + columnNames[i].rename + "</th>";

                break;
        }
    }

    if (makeRowEditable)
        table += "<th data-type=\"edit\">&nbsp;</th>";

    table += "</tr></thead><tbody>";

    if (jsonObject.length > 0) {

        for (var i = 0; i < jsonObject.length; i++) {
            var isRowSelected = false;

            if (arraySelectedRow != null) {
                var checkValue = null;

                for (var j = 0; j < columnNames.length; j++) {
                    if (columnNames[j].type == "primarycheckbox") {
                        checkValue = jsonObject[i][columnNames[j].name];
                        break;
                    }
                }

                if (checkValue != null) {
                    for (var k = 0; k < arraySelectedRow.length; k++) {
                        if (arraySelectedRow[k] == checkValue) {
                            isRowSelected = true;
                            break;
                        }
                    }
                }
            }

            var bgcolor = "";
            var fgcolor = "";

            for (var z = 0; z < columnNames.length; z++) {
                switch (columnNames[z].type) {
                    case "bgcolor":
                        bgcolor = jsonObject[i][columnNames[z].name];
                        break;

                    case "fgcolor":
                        fgcolor = jsonObject[i][columnNames[z].name];
                        break;
                }

                if ((bgcolor != "") && (fgcolor != ""))
                    break;
            }

            var trStyle = "";

            if ((bgcolor != "") && (bgcolor != null))
                trStyle = "background-color:" + bgcolor + ";";

            if ((fgcolor != "") && (fgcolor != null))
                trStyle += "color:" + fgcolor + ";";

            table += "<tr " + (trStyle == "" ? "" : "style=\"" + trStyle + "\" ") + "class=\"" + (isRowSelected ? "Selected" : "NotSelected") + "\">";
            var editID = "";
            for (var j = 0; j < columnNames.length; j++) {
                switch (columnNames[j].type) {
                    case "checkbox":
                        var checkClass = tableId + columnNames[j].name.replace(/ /g, "_") + "checkbox";
                        table += "<td class=\"ListViewCheckbox\"><input type=\"checkbox\" class=\"" + checkClass + "\"" +
                            (columnNames[j].onclick != null ? " onclick=\"" + columnNames[j].onclick + "\"" : "") +
                            (columnNames[j].valueColumn != null ? " value=\"" + jsonObject[i][columnNames[j].valueColumn] + "\"" : "") +
                            (jsonObject[i][columnNames[j].name] == "1" ? " checked" : "") + "/></td>";
                        break;

                    case "primarycheckbox":
                        var checkClass = tableId + columnNames[j].name.replace(/ /g, "_") + "checkbox";
                        table += "<td class=\"ListViewCheckbox\"><input type=\"checkbox\" onclick=\"checkboxClicked(this);\" class=\"" + checkClass + "\" value=\"" + jsonObject[i][columnNames[j].name] + "\"" + (isRowSelected ? " checked" : "") + "/></td>";
                        break;

                    case "primaryradio":
                        var checkClass = tableId + columnNames[j].name.replace(/ /g, "_") + "checkbox";
                        table += "<td class=\"ListViewCheckbox\"><input type=\"radio\" class=\"" + checkClass + "\" name=\"" + checkClass + "\" value=\"" + jsonObject[i][columnNames[j].name] + "\"/></td>";
                        break;

                    case "serialnumber":
                        table += "<td class=\"ListViewSerialNumber\">" + (i + 1) + "</td>";
                        break;

                    case "bgcolor":
                    case "fgcolor":
                        break;

                    case "editable":
                        editID = jsonObject[i][columnNames[j].name];
                        break;

                    default:
                        var data = jsonObject[i][columnNames[j].name];
                        var tdStyle = "";

                        if ((columnNames[j].bgcolor != null) && (columnNames[j].bgcolor != "")) {
                            bgcolor = jsonObject[i][columnNames[j].bgcolor];
                            if ((bgcolor != "") && (bgcolor != null))
                                tdStyle = "background-color:" + bgcolor + ";";
                        }

                        if ((columnNames[j].fgcolor != null) && (columnNames[j].fgcolor != "")) {
                            fgcolor = jsonObject[i][columnNames[j].fgcolor];
                            if ((fgcolor != "") && (fgcolor != null))
                                tdStyle += "color:" + fgcolor + ";";
                        }

                        if (data instanceof Array)
                            table += "<td" + (tdStyle == "" ? "" : " style=\"" + tdStyle + "\" ") + ">" + data.join("<br/>") + "</td>";
                        else {
                            if (data instanceof String)
                                data = data.replace(/\r\n/g, "</br>").replace(/\n/g, "</br>");

                            table += "<td" + (tdStyle == "" ? "" : " style=\"" + tdStyle + "\" ") + ">" + data + "</td>";
                        }
                        break;
                }
            }

            if (makeRowEditable)
                table += "<td><a href=\"#\" data-value=\"" + editID + "\"  onclick=\"editListViewEntry(this," + ModalPopupId + ",'" + editableFunction + "'); return false;\">Edit</a></td>";

            table += "</tr>";
        }

        table += "</tbody>"

        if (!hideFooterCount)
            table += "<tfoot><tr><td colspan=\"" + columnNames.length + "\">Total " + jsonObject.length + " items.</td></tr></tfoot>"

        table += "</table></div>";
    }
    else
        table += "</tbody><tfoot><tr><td colspan=\"" + columnNames.length + "\" align=\"center\">No items available to display.</td></tr></tfoot></table></div>";

    objPlaceHolder.innerHTML = table;

    if (makeSortable) {
        var objTable = document.getElementById(tableId);
        sorttable.makeSortable(objTable);
    }

    if (makeRowEditable) {
        objPlaceHolder.innerHTML += "<div id=\"" + ModalPopupId + "\"></div>";

        convertDivToModalPopup(document.getElementById(ModalPopupId));
    }
}

function toggleSelect(objCheckAll) {
    $("." + objCheckAll.className).each(function (i, value) {
        if (value != objCheckAll) {
            value.checked = objCheckAll.checked;
            checkboxClicked(value);
        }
    });
}

function checkboxClicked(checkbox) {
    var tr = checkbox.parentNode.parentNode;

    if (checkbox.checked) {
        var bgcolor = $(tr).css("background-color");

        if (bgcolor != 'rgb(241, 234, 234)') {
            $(tr).css("background-color", "");
            $(tr).attr("data-oldColor", bgcolor);
        }

        tr.className = "Selected";
    }
    else {
        tr.className = "NotSelected";

        var bgcolor = $(tr).attr("data-oldColor");
        $(tr).css("background-color", bgcolor);
    }
}

function getCheckedValues(objPlaceHolder, columnName) {
    var data = "";

    $("td ." + objPlaceHolder.id + "table" + columnName.replace(/ /g, "_") + "checkbox").each(function (i, value) {
        if (value.checked)
            data += value.value + ",";
    });

    if (data.length > 0)
        data = data.substr(0, data.length - 1);

    return data;
}

function editListViewEntry(obj, objPopup, onsucess) {
    var objtr = obj.parentNode.parentNode;
    var objHeadTr = objtr.parentNode.parentNode.firstChild.firstChild;

    var data = new Array();
    var THCount = 0;

    for (var i = 0; i < objHeadTr.childNodes.length; i++) {
        var objTH = objHeadTr.childNodes[i];

        if (objTH.nodeName == "TH") {
            var type = objTH.getAttribute("data-type");
            switch (type) {
                case "serialnumber":
                case "primaryradio":
                case "checkbox":
                case "bgcolor":
                case "primarycheckbox":
                case "fgcolor":
                case "bgcolor":
                case "edit":
                    break;

                default:
                    var TDCount = 0;

                    for (var j = 0; j < objtr.childNodes.length; j++) {
                        var objTD = objtr.childNodes[j];

                        if (objTD.nodeName == "TD") {

                            if (THCount == TDCount) {
                                var childlenght = $(objTD).children().length;
                                var nodedata;
                                if (childlenght > 0) {
                                    var childnode = objTD.childNodes[0];
                                    nodedata = childnode.innerHTML;
                                }
                                else
                                    nodedata = objTD.innerHTML;

                                if (type == "dropdown")
                                    data.push({ "column": objTH.innerHTML, "type": type, "data": nodedata, options: JSON.parse(unescape(objTH.getAttribute("data-options"))) });
                                else
                                    data.push({ "column": objTH.innerHTML, "type": type, "data": nodedata });
                                break;
                            }

                            TDCount++;
                        }
                    }
                    break;
            }

            THCount++;
        }
    }

    var popupBody = document.getElementById(objPopup.id + "ModalBody");

    popupBody.innerHTML = generateForm(data);

    var id = $(obj).attr('data-value');

    showStaticModalPopupEditable(objPopup, "Edit", 600, "Save", onsucess, id);
}
