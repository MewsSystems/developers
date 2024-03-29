import { Subscription } from "rxjs";
import { injectable } from "inversify";

@injectable()
export class Disposable {
    protected readonly _disposeBag: Set<Subscription> = new Set();

    dispose(): void {
        this._disposeBag.forEach(sub => sub.unsubscribe());
    }
}