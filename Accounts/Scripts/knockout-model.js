var user = $("#user");

function PaymentViewModel() {
    var self = this;
    self.payments = ko.observableArray([]);
    self.Amount = ko.observable(0);
    self.RemainingEachMonth = ko.observable(0);
    self.YearlyAmountEachMonth = ko.observable(0);
    self.CurrentAmountToPayThisMonth = ko.observable(0);
    self.TotalAmountToPayThisMonth = ko.observable(0);

    //form fields
    self.AmountToAdd = ko.observable(0);
    self.PaidYearly = ko.observable();
    self.Recurring = ko.observable();
    self.Title = ko.observable(0);
    self.Date = ko.observable();
    self.Recurring = ko.observable();

    self.AddPayment = function () {

        var model = {
            IncomeId: user.val(),
            Amount: self.AmountToAdd(),
            PaidYearly: self.PaidYearly(),
            Title: self.Title(),
            Date: self.Date(),
            Recurring: self.Recurring()
        };

        $.ajax("/Home/AddPayment", {
            data: JSON.stringify(model),
            type: "post",
            contentType: "application/json",
            success: function () {
                getPayments(user.val());
                self.AmountToAdd(0);
                self.PaidYearly(false);

                var today = getDate(new Date());

                self.Date(today.fullDate);
                self.Title("");

                $(".alert").show();
                $(".addPaymentLink").click();
            }
        });

    };

    self.ConfirmDeletePayment = function () {

        var model = {
            paymentId: $("#paymentIdHid").val()
        };

        $.ajax("/Home/DeletePayment", {
            data: JSON.stringify(model),
            type: "post",
            contentType: "application/json",
            success: function () {
                $('.modal').hide();
                $("#paymentIdHid").val("");
                getPayments(user.val());
            }
        });
    };

    self.CloseDeletePayment = function () {
        $('.modal').hide();
        $("#paymentIdHid").val("");
    };

    self.displayMode = function (entry) {
        return entry.edit() ? "edit-template" : "view-template";
    };

    function getPayments(userId) {
        $.getJSON("/Home/GetPaymentSummaryById/", {
            "userGuid": userId

        }, function (allData) {
            self.Amount(allData.Amount);
            self.RemainingEachMonth(allData.RemainingEachMonth);
            self.YearlyAmountEachMonth(allData.YearlyAmountEachMonth);
            self.CurrentAmountToPayThisMonth(allData.CurrentAmountToPayThisMonth);
            self.TotalAmountToPayThisMonth(allData.TotalAmountToPayThisMonth);

            var mappedPayments = $.map(allData.Payments, function (item) { return new payment(item); });
            self.payments(mappedPayments);
        });

    }

    function getDate(date) {

        var newDate = [];
        newDate.month = date.getMonth() + 1;
        newDate.day = date.getDate();
        newDate.year = date.getFullYear();

        if (newDate.day < 10) {
            newDate.day = '0' + newDate.day;
        }
        if (newDate.month < 10) {
            newDate.month = '0' + newDate.month;
        }

        newDate.fullDate = newDate.year + "-" + newDate.month + "-" + newDate.day;

        return newDate;
    }

    function payment(data) {
        this.Title = ko.observable(data.Title + " " +
            (data.PaidYearly === true ? "<span class=\"label label-warning\">Paid Yearly</span>" : "") +
            (data.Recurring === true ? "<span class=\"label label-info\">Recurring</span>" : ""));
        this.PaymentId = ko.observable(data.Id);
        this.Amount = ko.observable(data.Amount);
        this.Paid = ko.observable(data.Paid);
        this.PaidYearly = ko.observable(data.PaidYearly);
        this.Recurring = ko.observable(data.Recurring);

        var currentDay = getDate(new Date(parseInt(data.Date.substr(6))));

        this.Day = ko.observable(currentDay.day);
        this.Date = ko.observable(currentDay.fullDate);
        this.Title = ko.observable(data.Title);
        var paid = this.Paid;
        this.alreadyPaid = ko.pureComputed(function () {
            return paid() === true ? "success" : "danger";
        }, self);


        this.DeletePayment = function (parent, currentData) {

            $("#paymentIdHid").val(currentData.PaymentId());
            $('.modal').show();
        };

        this.edit = ko.observable(false);

        this.EditPayment = function (parent, currentData) {
            var beingEdited = currentData.edit();
            currentData.edit(!beingEdited);
        };
        this.UpdatePayment = function (parent, currentData) {

            var model = {
                Id: currentData.PaymentId(),
                Amount: currentData.Amount(),
                PaidYearly: currentData.PaidYearly(),
                Title: currentData.Title(),
                Date: currentData.Date(),
                Recurring: currentData.Recurring()
            };

            $.ajax("/Home/UpdatePayment", {
                data: JSON.stringify(model),
                type: "post",
                contentType: "application/json",
                success: function () {
                    getPayments(user.val());
                    currentData.edit(false);
                }
            });

        };
        this.CancelPayment = function (parent, currentData) {
            currentData.edit(false);
        };
    }

    user.change(function () {
        getPayments(user.val());
    });

    getPayments(user.val());
}

ko.applyBindings(new PaymentViewModel());

