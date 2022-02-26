
//const staticFilesToCache = [
//    '/',
//    '/home/shop',
//    '/home/About',
//    '/home/Contact',
//    '/home/cart',
//    '/home/addtocart',
//    'account',
//    '/css/bootstrap.min.css',
//    '/css/custom.css',
//    '/css/fontawesome.css',
//    '/css/fontawesome.min.css',
//    '/css/templatemo.css',
//    '/css/templatemo.min.css'
   

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








