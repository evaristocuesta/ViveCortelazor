// Ensure the dataLayer exists
window.dataLayer = window.dataLayer || [];

function setCookie(name, value, days) {
    var now = new Date();
    var time = now.getTime();
    var expireTime = time + 1000 * 3600 * 24 * days;
    now.setTime(expireTime);

    document.cookie = name + '=' + value + ';expires=' + now.toGMTString() + ';path=/;domain=' + window.location.hostname + ';';
}


function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');

    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].trim(); // Ensure trimming any leading spaces
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length);
    }

    return null;
}

function setCookieConsent() {

    var consentValues = ['Necessary']; // Necessary cookies are always accepted.

    if (document.getElementById('preferencesCookie').checked) {
        consentValues.push('Preferences');
    }
    if (document.getElementById('statisticalCookie').checked) {
        consentValues.push('Statistical');
    }
    if (document.getElementById('marketingCookie').checked) {
        consentValues.push('Marketing');
    }

    var consentValue = consentValues.join(',');
    var existingConsent = readCookie('cookieConsent');

    // Update the cookie and the dataLayer only if the consent has changed.
    if (consentValue !== existingConsent) {
        setCookie('cookieConsent', consentValue, 30);

        window.dataLayer.push({
            'event': 'cookie_consent_update',
            'cookieConsent': consentValue
        });
    }

    hideConsentBanner();
    showMinimizedBanner();
}

// Show or hide banners based on cookie consent
window.onload =  function () {
    var cookieValue = readCookie('cookieConsent');

    if (cookieValue) {
        // Initialize checkbox states based on cookie value.
        document.getElementById('preferencesCookie').checked = cookieValue.includes('Preferences');
        document.getElementById('statisticalCookie').checked = cookieValue.includes('Statistical');
        document.getElementById('marketingCookie').checked = cookieValue.includes('Marketing');
        showMinimizedBanner();
    } else {
        document.getElementById('cookieConsentBanner').style.display = 'block';
    }
};

function hideConsentBanner() {
    document.getElementById('cookieConsentBanner').style.display = 'none';
}

function showMinimizedBanner() {
    document.getElementById('minimizedConsentBanner').style.display = 'block';
}

function acceptAll() {
    document.getElementById('preferencesCookie').checked = true;
    document.getElementById('statisticalCookie').checked = true;
    document.getElementById('marketingCookie').checked = true;
    setCookieConsent();
}

function rejectAll() {
    document.getElementById('preferencesCookie').checked = false;
    document.getElementById('statisticalCookie').checked = false;
    document.getElementById('marketingCookie').checked = false;
    setCookieConsent();
}

function openConsentBanner() {
    document.getElementById('cookieConsentBanner').style.display = 'block';
    document.getElementById('minimizedConsentBanner').style.display = 'none';
}