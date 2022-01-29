////(function () {
////    // console.log('1');
////    if ('serviceWorker' in navigator) {
////        // console.log('2');//('/PWA/service-worker.js', {scope: '/PWA/'}) for github
////        window.addEventListener('load', () => {
////            navigator.serviceWorker.register('/serviceWorker.js')
////                .then(registration => {
////                    console.log('Service Worker is registered', registration);
                    
////                })
////                .catch(err => {
////                    console.log('Registration failed:', err);
////                });
////        });
////    }
////    let deferredPrompt;
////    var addBtn = document.getElementById('installBtn');
////    window.addEventListener('beforeinstallprompt', (e) => {
////        console.log(`beforeinstallprompt`);
////        // Prevent Chrome 67 and earlier from automatically showing the prompt
////        e.preventDefault();
////        // Stash the event so it can be triggered later.
////        deferredPrompt = e;
////        // Update UI to notify the user they can add to home screen
////        addBtn.style.display = 'block';

////    });



////    addBtn.addEventListener('click', (e) => {
////        // hide our user interface that shows our A2HS button
////        addBtn.style.display = 'none';
////        // Show the prompt
////        deferredPrompt.prompt();
////        // Wait for the user to respond to the prompt
////        deferredPrompt.userChoice.then((choiceResult) => {
////            if (choiceResult.outcome === 'accepted') {
////                console.log('User accepted the A2HS prompt');
////            } else {
////                console.log('User dismissed the A2HS prompt');
////            }
////            deferredPrompt = null;
////        });
////    });
////})();

