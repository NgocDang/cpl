abstract class Base {
    // The abstract method the subclass will have to call
    protected abstract init(): void;

    constructor() {
        jQuery(document).ready(() => {
            this.init();
        });
    }
}