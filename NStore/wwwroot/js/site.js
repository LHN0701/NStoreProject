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

                $.each(res, function (i, item) {
                    numberItem += item.quantity;
                });
                $('.lbl_number_items_header').text(numberItem);
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

                    $.each(res, function (i, item) {
                        numberItem += item.quantity;
                    });
                    $('.lbl_number_items_header').text(numberItem);
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