export const createLoop = (model, present, setState) => {
	return (actionPromise) => {
		actionPromise.then(proposal => {
			console.log(proposal);
			return model.accept(proposal);
		}).then(model => {
			return present(model);
		}).then(state => {
			console.log(state);
			return setState(state);
		})
	}
};