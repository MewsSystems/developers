import { Heart } from "lucide-react";

export default function Footer() {
    return (
        <footer className="mt-20 border-t border-white/10 bg-black/60">
            <div className="container py-10 text-center text-xs text-neutral-400 flex flex-col items-center gap-2">
                <p>© {new Date().getFullYear()} CinEmir Movies</p>
                <p className="flex items-center gap-1 text-neutral-500">
                    Made with{" "}
                    <Heart className="text-[#00ad99] w-4 h-4" aria-hidden /> by{" "}
                    <span className="font-medium text-neutral-300">
                        Emir Bayraktar
                    </span>
                </p>
            </div>
        </footer>
    );
}
