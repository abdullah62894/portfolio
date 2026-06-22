// ============================================
// Portfolio Site - Interactive Functions
// ============================================

// --- Smooth scroll to section ---
window.scrollToSection = function (sectionId) {
    const element = document.getElementById(sectionId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
};

// --- Download resume ---
window.downloadResume = function () {
    const link = document.createElement('a');
    link.href = 'assets/Abdullah Hammad - Optimized Cv.pdf';
    link.download = 'Abdullah Hammad - Optimized Cv.pdf';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

// --- Open email client ---
window.openEmail = function () {
    window.location.href = 'mailto:abdullahhammad14@gmail.com';
};

// --- Open LinkedIn ---
window.openLinkedIn = function () {
    window.open('https://www.linkedin.com/in/abdullahhammad14/', '_blank', 'noopener');
};

// --- Initialize portfolio animations ---
window.initPortfolio = function () {
    const revealElements = document.querySelectorAll('.reveal, .reveal-left, .reveal-right, .reveal-scale');

    const observer = new IntersectionObserver(
        function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add('visible');
                }
            });
        },
        { threshold: 0.1 }
    );

    revealElements.forEach(function (el) {
        observer.observe(el);
    });
};

// --- Scroll listener (Navbar scrolled class & active navigation link) ---
window.addEventListener('scroll', function () {
    const navbar = document.getElementById('navbar');
    if (navbar) {
        if (window.scrollY > 50) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }
    }

    const sections = document.querySelectorAll('.section[id]');
    const navLinks = document.querySelectorAll('.nav-link');
    if (sections.length > 0 && navLinks.length > 0) {
        let current = '';
        sections.forEach(function (section) {
            const top = section.offsetTop - 120;
            if (window.scrollY >= top) {
                current = section.getAttribute('id');
            }
        });

        navLinks.forEach(function (link) {
            link.classList.remove('active');
            if (link.getAttribute('href') === '#' + current) {
                link.classList.add('active');
            }
        });
    }
});

// Fallback init on DOM content load
document.addEventListener('DOMContentLoaded', function () {
    window.initPortfolio();
});
