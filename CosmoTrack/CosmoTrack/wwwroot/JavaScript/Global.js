$('.review').hide();

$('.parent').on('click', function (event) {

    $(this).next().show();

})
$('.review').on('click', function (event) {
    $(this).hide();
    $('.seeReview').show();
})

$('.seeReview').on('click', function (event) {

    $(this).hide();

})

function createYouTubeEmbedLink(link) {
    return link.replace("http://www.youtube.com/watch?v=", "http://www.youtube.com/embed/");
}