
//const staticFilesToCache = [
//    '/',
//    '/home/shop',
//    '/home/About',
//    '/home/Contact',
//    '/css/bootstrap.min.css',
//    '/css/custom.css',
//    '/css/fontawesome.css',
//    '/css/fontawesome.min.css',
//    '/css/slick-theme.css',
//    '/css/slick-theme.min.css',
//    '/css/slick.min.css',
//    '/css/templatemo.css',
//    '/css/templatemo.min.css',
//    '/js/bootstrap.bundle.min.js',
//    '/js/custom.js',
//    '/js/jquery-1.11.0.min.js',
//    '/js/jquery-migrate-1.2.1.min.js',
//    '/js/slick.min.js',
//    '/js/templatemo.js',
//    '/js/templatemo.min.js',
//    '/webfonts/fa-brands-400.eot',
//    '/webfonts/fa-brands-400.svg',
//    '/webfonts/fa-brands-400.ttf',
//    '/webfonts/fa-brands-400.woff',
//    '/webfonts/fa-brands-400.woff2',
//    '/webfonts/fa-regular-400.eot',
//    '/webfonts/fa-regular-400.svg',
//    '/webfonts/fa-regular-400.ttf',
//    '/webfonts/fa-regular-400.woff',
//    '/webfonts/fa-regular-400.woff2',
//    '/webfonts/fa-solid-900.eot',
//    '/webfonts/fa-solid-900.svg',
//    '/webfonts/fa-solid-900.ttf',
//    '/webfonts/fa-solid-900.woff',
//    '/webfonts/fa-solid-900.woff2',
//    '/webfonts/slick.eot',
//    '/webfonts/slick.svg',
//    '/webfonts/slick.ttf',
//    '/webfonts/slick.woff'

//];
//const staticCacheName = 'pages-cache-v1.3';
//const dynamicFilesToCache = 'dynamic-cahe-v1';
//self.addEventListener('install', event => {
//    console.log('install');
//    self.skipWaiting();
//    event.waitUntil(
//        caches.open(staticCacheName)
//            .then(async cache => {
//                console.log('cache');
//                return await cache.addAll(staticFilesToCache);
//            })
//    );

//});

//self.addEventListener('activate', event => {
//    console.log('Activating new service worker ... ');

//    const cacheAllowlist = [staticCacheName];
//    event.waitUntil(
//        caches.keys()
//            .then(cacheNames => {
//                return Promise.all(
//                    cacheNames.map(cacheName => {
//                        if (cacheAllowlist.indexOf(cacheName) === -1) {
//                            return caches.delete(cacheName);
//                        }
//                    })
//                );
//            })
//    );
//});
//self.addEventListener('fetch', (e) => {

//    e.respondWith((async () => {
//        const r = await caches.match(e.request);
//        console.log(`[Service Worker] Fetching resource: ${e.request.url}`);
//        if (r) { return r; }
//        const response = await fetch(e.request);
//        const cache = await caches.open(dynamicFilesToCache);
//        //console.log(`[Service Worker] Caching new resource: ${e.request.url}`);
//        cache.put(e.request, response.clone());
//        return response;
//    })());
//});








