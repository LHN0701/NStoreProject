var SiteController = function () {
    this.initialize = function () {
        registerEvents();
        loadCart();
    }
    function loadCart() {
        $.ajax({
            type: "GET",
            url: '/Cart/GetListItem',
            success: function (res) {
                var numberItem = 0;
                var priceItem = 0;

                $.each(res, function (i, item) {
                    numberItem += item.quantity;
                    priceItem += item.price * item.quantity;
                });
                $('.lbl_number_items_header').text(numberItem);
                $('.lbl_price_items_header').text("$" + priceItem);
            }
        });
    }

    function registerEvents() {
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');

            $.ajax({
                type: "POST",
                url: '/Cart/AddToCart',
                data: {
                    id: id
                },
                success: function (res) {
                    var numberItem = 0;
                    var priceItem = 0;

                    $.each(res, function (i, item) {
                        numberItem += item.quantity;
                        priceItem += item.price * item.quantity;
                    });
                    $('.lbl_number_items_header').text(numberItem);
                    $('.lbl_price_items_header').text("$" + priceItem);
                },
                error: function (err) {
                    console.log(err)
                }
            })
        })
    }
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}