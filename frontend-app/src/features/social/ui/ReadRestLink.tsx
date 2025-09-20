export function ReadRestLink({ open, onClick }: { open: boolean, onClick: () => void }) {
    if (!open) {
        return (<span onClick={onClick}><a href="javascript:void(0)">...read the rest</a></span>);
    }
    return <></>
}
