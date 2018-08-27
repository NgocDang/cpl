class Contact extends Base {

    // Define here the abstract method
    public init() {
        this.bindDoSend();
    };

    bindDoSend() {
        $("#btn-do-send").on("click", function () {
            var _this = this;
            var formContact = ($("#form-contact")[0]) as any;
            var isFormValid = formContact.checkValidity();
            $("#form-contact").addClass('was-validated');

            var isCategoryValid = $("#Category").val() != "";
            if (isCategoryValid)
                $("#category-error").hide();
            else
                $("#category-error").show();

            if (isFormValid && isCategoryValid) {
                var service = new Ajax.Service();
                var ajaxData = {
                    Email: $("#Email").val(),
                    Description: $("#Description").val(),
                    Subject: $('#Subject').val(),
                    Category: $("#Category").val()
                };
                var option = new Ajax.Option("/Contact/DoSend/", "POST", undefined, undefined, ajaxData);
                service.request(option,
                    (data ) => {
                        if (data.success) {
                            $("#form-contact").hide();
                            $("#contact-response").show();
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    (data) => {
                        if (data.success) {
                            $("#form-contact").hide();
                            $("#contact-response").show();
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    (data) => {
                        if (data.success) {
                            $("#form-contact").hide();
                            $("#contact-response").show();
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    }
                )
                $.ajax({
                    url: "/Contact/DoSend/",
                    type: "POST",
                    beforeSend: function () {
                        $(_this).prop("disabled", true);
                        $(_this).html("<i class='fa fa-spinner fa-spin'></i> <i class='la la-check'></i> " + $(_this).text().trim());
                    },
                    data: {
                        Email: $("#Email").val(),
                        Description: $("#Description").val(),
                        Subject: $('#Subject').val(),
                        Category: $("#Category").val()
                    },
                    success: function (data) {
                        if (data.success) {
                            $("#form-contact").hide();
                            $("#contact-response").show();
                            toastr.success(data.message, 'Success!');
                        } else {
                            toastr.error(data.message, 'Error!');
                        }
                    },
                    complete: function () {
                        $(_this).prop("disabled", false);
                        $(_this).html("<i class='la la-check'></i> " + $(_this).text().trim());
                    }
                });
            }
            return false;
        });
    }
};

new Contact();


module Ajax {
    export class Option {
        url: string;
        method: string;
        processData?: boolean;
        contentType?: boolean;
        data: any;
        constructor(url: string, method?: string, processData?: boolean, contentType?: boolean, data?: any) {
            this.url = url;
            this.method = method || "get";
            this.processData = processData || undefined;
            this.contentType = contentType || undefined;
            this.data = data || {};
        }
    }

    export class Service {
        public request = (options: Option, success: Function, beforeSend: Function, complete: Function): void => {
            $.ajax({
                url: options.url,
                type: options.method,
                processData: options.processData,
                contentType: options.contentType,
                data: options.data,
                success: function (data) {
                    success(data);
                },
                beforeSend: function (data) {
                    beforeSend(data);
                },
                complete: function (data) {
                    complete(data);
                }
            });
        }
    }
}

//export class Options {
//    url: string;
//    method: string;
//    data: Object;
//    constructor(url: string, method?: string, data?: Object) {
//        this.url = url;
//        this.method = method || "get";
//        this.data = data || {};
//    }
//}

//export class Service {
//    public request = (options: Options, successCallback: Function, errorCallback?: Function): void => {
//        $.ajax({
//            url: options.url,
//            type: options.method,
//            data: options.data,
//            cache: false,
//            success: function (d) {
//                successCallback(d);
//            },
//            error: function (d) {
//                if (errorCallback) {
//                    errorCallback(d);
//                    return;
//                }
//                var errorTitle = "Error in (" + options.url + ")";
//                var fullError = JSON.stringify(d);
//                console.log(errorTitle);
//                console.log(fullError);
//            }
//        });
//    }
//}