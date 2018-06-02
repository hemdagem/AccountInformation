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
    self.Title = ko.observable();
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

        $.ajax("/Payments/AddPayment", {
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

        $.ajax("/Payments/DeletePayment", {
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
        $.getJSON("/Payments/GetPaymentSummaryById/", {
            "userGuid": userId

        }, function (allData) {
            self.Amount(allData.amount);
            self.RemainingEachMonth(allData.remainingEachMonth);
            self.YearlyAmountEachMonth(allData.yearlyAmountEachMonth);
            self.CurrentAmountToPayThisMonth(allData.currentAmountToPayThisMonth);
            self.TotalAmountToPayThisMonth(allData.totalAmountToPayThisMonth);

            var mappedPayments = $.map(allData.payments, function (item) { return new payment(item); });
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
        this.Title = ko.observable(data.title);
        this.Type =
            (data.paidYearly === true ? "<span class=\"label label-warning\">Paid Yearly</span>" : "") +
            (data.recurring === true ? "<span class=\"label label-info\">Recurring</span>" : "");
        this.PaymentId = ko.observable(data.id);
        this.Amount = ko.observable(data.amount);
        this.Paid = ko.observable(data.paid);
        this.PaidYearly = ko.observable(data.paidYearly);
        this.Recurring = ko.observable(data.recurring);

        var currentDay = getDate(new Date(parseInt(data.date.substr(6))));

        this.Day = ko.observable(currentDay.day);
        this.Date = ko.observable(currentDay.fullDate);
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

            $.ajax("/Payments/UpdatePayment", {
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

