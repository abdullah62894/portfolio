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
    // Placeholder - replace with actual resume URL when available
    // window.open('/files/abdullah_hammad_resume.pdf', '_blank');
    alert('Resume download will be available soon. Please contact me via email for a copy.');
};

// --- Open email client ---
window.openEmail = function () {
    window.location.href = 'mailto:abdullahhammad14@gmail.com';
};

// --- Open LinkedIn ---
window.openLinkedIn = function () {
    window.open('https://www.linkedin.com/in/abdullahhammad14/', '_blank', 'noopener');
};

// --- Navbar scroll effect ---
document.addEventListener('DOMContentLoaded', function () {
    const navbar = document.getElementById('navbar');

    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) {
            navbar.classList.add('scrolled');
        } else {
            navbar.classList.remove('scrolled');
        }
    });

    // --- Intersection Observer for reveal animations ---
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

    // --- Active nav link highlighting ---
    const sections = document.querySelectorAll('.section[id]');
    const navLinks = document.querySelectorAll('.nav-link');

    window.addEventListener('scroll', function () {
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
    });
});
