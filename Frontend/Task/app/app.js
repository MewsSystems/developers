import AppCore from './src/AppCore';

export function run(element) {
    console.log('App is running.');

    (AppCore.instance).render();
}
