//$(document).ready(function() {
//    const dataUrl = 'https://api.example.com/orders'; // Replace with actual data URL

//    $.getJSON(dataUrl, function(data) {
//        data.forEach(order => {
//            const card = `
//                <div class="card">
//                    <img src="${order.image}" alt="${order.store}">
//                    <div class="card-details">
//                        <strong>${order.store}</strong> -> ${order.product}
//                        <div>${order.description}</div>
//                        <div>Order Date: ${order.orderDate}</div>
//                        <div>Price: ${order.price}</div>
//                    </div>
//                    <div class="card-actions">
//                        <button>${order.status}</button>
//                    </div>
//                </div>
//            `;
//            $('#card-container').append(card);
//        });
//    });
//});