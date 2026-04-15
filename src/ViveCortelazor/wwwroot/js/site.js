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
        setCookie('cookieConsent', consentValue, 365);

        // Cleanup cookies that are no longer consented to
        cleanupCookies(consentValues);

        // Update Google Consent Mode v2
        updateConsentMode(consentValues);

        window.dataLayer.push({
            'event': 'cookie_consent_update',
            'cookieConsent': consentValue
        });
    }

    hideConsentBanner();
    showMinimizedBanner();
}

function updateConsentMode(consentValues) {
    // Helper function for gtag
    function gtag() { dataLayer.push(arguments); }

    // Mapping your categories to Google Consent Mode v2 categories
    var consentUpdate = {
        'ad_storage': consentValues.includes('Marketing') ? 'granted' : 'denied',
        'ad_user_data': consentValues.includes('Marketing') ? 'granted' : 'denied',
        'ad_personalization': consentValues.includes('Marketing') ? 'granted' : 'denied',
        'analytics_storage': consentValues.includes('Statistical') ? 'granted' : 'denied',
        'functionality_storage': consentValues.includes('Preferences') ? 'granted' : 'denied',
        'personalization_storage': consentValues.includes('Preferences') ? 'granted' : 'denied',
        'security_storage': 'granted'  // Siempre permitido
    };

    // Update the consent
    gtag('consent', 'update', consentUpdate);
}

// Show or hide banners based on cookie consent
window.onload = function () {
    cookiesSetup();
    contactInfo();
};

function cookiesSetup() {
    var cookieValue = readCookie('cookieConsent');

    if (cookieValue) {
        // Initialize checkbox states based on cookie value.
        document.getElementById('preferencesCookie').checked = cookieValue.includes('Preferences');
        document.getElementById('statisticalCookie').checked = cookieValue.includes('Statistical');
        document.getElementById('marketingCookie').checked = cookieValue.includes('Marketing');

        // Restore consent in Google Consent Mode
        var consentValues = cookieValue.split(',');
        updateConsentMode(consentValues);

        showMinimizedBanner();
    } else {
        document.getElementById('cookieConsentBanner').style.display = 'block';
    }

    document.getElementById('minimizedConsentBanner').onclick = openConsentBanner;
    document.getElementById('minimizedConsentBanner').onkeydown = openConsentBanner;
}

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

function contactInfo() {
    const emailAddress = atob('aW5mb0B2aXZlY29ydGVsYXpvci5lcw==');
    const emails = document.querySelectorAll('a.email-contact-info');
    emails.forEach(email => {
        email.setAttribute('href', 'mailto:' + emailAddress);
    });
}

function deleteCookie(name) {
    // Delete cookie in the current domain
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';

    // Delete cookie by specifying the domain explicitly
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; domain=' + window.location.hostname + ';';

    // If the domain has subdomains, also try deleting with a leading dot
    if (window.location.hostname.indexOf('.') > -1) {
        var rootDomain = '.' + window.location.hostname.split('.').slice(-2).join('.');
        document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; domain=' + rootDomain + ';';
    }
}

function cleanupCookies(consentValues) {
    // List of cookies belonging to each category
    var cookieCategories = {
        'Preferences': [
            // Add your preference cookies here
            // Example: 'language_preference', 'theme_preference'
        ],
        'Statistical': [
            // Google Analytics cookies
            '_ga',
            '_ga_' + 'G-W86DLX5WN3'.split('-')[1],
            '_gid',
            '_gat',
            '_gat_gtag_' + 'G-W86DLX5WN3'.replace(/-/g, '_'),
        ],
        'Marketing': [
            // Marketing/advertising cookies
            '_gcl_au',
            '_fbp',
            '_ttp',
            // Google Ads
            'test_cookie',
            'IDE',
            'DSID',
            'id',
            '1P_JAR',
            'CONSENT',
            'NID',
        ]
    };

    // Delete cookies of categories not accepted
    for (var category in cookieCategories) {
        if (!consentValues.includes(category)) {
            cookieCategories[category].forEach(function (cookieName) {
                deleteCookie(cookieName);
            });
        }
    }
}
