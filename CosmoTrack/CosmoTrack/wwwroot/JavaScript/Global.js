$('.review').hide();

$('.parent').on('click', function (event) {

    $(this).next().show();

})

$('.seeReview').on('click', function (event) {

    $(this).hide();

})
$('.review').on('click', function (event) {
    $(this).hide();
    $('.seeReview').show();
})


