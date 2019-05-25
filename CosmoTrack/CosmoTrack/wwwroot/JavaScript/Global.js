$('.review').hide();

$('.parent').on('click', function (event) {

    $(this).next().show();

    $('.seeReview').hide();
})
$('.review').on('click', function (event) {
    $(this).hide();
    $('.seeReview').show();
})


