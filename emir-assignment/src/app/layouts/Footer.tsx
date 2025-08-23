export default function Footer() {
    return (
        <footer className=" mt-20 border-t border-white/10 bg-black/60">
            <div className="text-center container py-10 text-xs text-neutral-400">
                © {new Date().getFullYear()} CinEmir — Built with React +
                Tailwind
            </div>
        </footer>
    );
}
