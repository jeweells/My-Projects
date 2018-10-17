
// Let start and end be an object with x and y coordinates
function findPath(matrix, start, end)
{
	let minSteps = null;
	function State() {
		this.steps = 0;
		this.path = [];
	}
	const firstState = new State();
	firstState.path.push(start);
	const pendingStates = [firstState];
	while(pendingStates.length != 0)
	{
		// Take the first element
		const currentState = pendingStates.shift();
		// Move to every possible position
		const csPos = currentState.path[currentState.path.length - 1];
		if(csPos.y == end.y && csPos.x == end.x)
		{
			minSteps = currentState.steps;
			break;
		}

		// Up
		if(csPos.y != 0 && !matrix[csPos.y-1][csPos.x] 
			&& currentState.path.find((e) => { return e.y == csPos.y - 1 && e.x == csPos.x; }) == undefined)
		{
			const newstate = new State();
			newstate.path = currentState.path.concat([{y: csPos.y-1, x: csPos.x}]);
			newstate.steps = currentState.steps + 1;
			pendingStates.push(newstate);
		}

		// Down
		if(csPos.y != matrix.length - 1 && !matrix[csPos.y+1][csPos.x] 
			&& currentState.path.find((e) => { return e.y == csPos.y + 1 && e.x == csPos.x; }) == undefined)
		{
			const newstate = new State();
			newstate.path = currentState.path.concat([{y: csPos.y+1, x: csPos.x}]);
			newstate.steps = currentState.steps + 1;
			pendingStates.push(newstate);
		}


		// Left
		if(csPos.x != 0 && !matrix[csPos.y][csPos.x-1] 
			&& currentState.path.find((e) => { return e.y == csPos.y && e.x == csPos.x-1; }) == undefined)
		{
			const newstate = new State();
			newstate.path = currentState.path.concat([{y: csPos.y, x: csPos.x-1}]);
			newstate.steps = currentState.steps + 1;
			pendingStates.push(newstate);
		}

		// Right
		if(csPos.x != matrix[0].length-1 && !matrix[csPos.y][csPos.x+1] 
			&& currentState.path.find((e) => { return e.y == csPos.y && e.x == csPos.x+1; }) == undefined)
		{
			const newstate = new State();
			newstate.path = currentState.path.concat([{y: csPos.y, x: csPos.x+1}]);
			newstate.steps = currentState.steps + 1;
			pendingStates.push(newstate);
		}

		pendingStates.sort((s1, s2) => {
			const posS1 = s1.path[s1.path.length - 1];
			const posS2 = s2.path[s2.path.length - 1];
			const distS1 = Math.abs(posS1.x - end.x) + Math.abs(posS1.y - end.y);
			const distS2 = Math.abs(posS2.x - end.x) + Math.abs(posS2.y - end.y);
			return distS1 - distS2;
		});
	}
	return minSteps;
}
