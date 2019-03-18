import { HttpUrlEncodingCodec } from '@angular/common/http';

export class CustomHttpUrlEncodingCodec extends HttpUrlEncodingCodec {
    encodeKey(key: string): string {
        return super.encodeKey(key)
            .replace(new RegExp("%5B", "g"), "[")
            .replace(new RegExp("%5D", "g"), "]");
    }
}