import { useTrackedGlobalState } from "../../utilities/store";

export const LoadingScreen = () => {
    const appState = useTrackedGlobalState();
    return (<p><em> {appState.generalString.loadingMessage + "..."}</em></p>);
};