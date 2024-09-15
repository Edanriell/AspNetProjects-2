//	The LSP states that in a program, if we replace an instance of a superclass (supertype) with an instance
//	of a subclass (subtype), the program should not break or behave unexpectedly.
//	Imagine we have a base class called Bird with a function called Fly, and we add the Eagle and Penguin
//	subclasses. Since a penguin can’t fly, replacing an instance of the Bird class with an instance of the
//	Penguin subclass might cause problems because the program expects all birds to be able to fly.
//	So, according to the LSP, our subclasses should behave so the program can still work correctly, even
//	if it doesn’t know which subclass it’s using, preserving system stability

//	Let ∅(𝑥𝑥) be        a property provable about          objects x of type T. Then, ∅(𝑦𝑦) should be true for
//	objects y of type S,         where S is a subtype of T
//	In simpler words, if S is a subtype of T, we can replace objects of type T with objects of type S without
//	changing any of the expected behaviors of the program (correctness).

//	In your subtypes, add new behaviors and states; don’t change existing ones.
//	In a nutshell, applying the LSP allows us to swap an instance of a class for one of its subclasses without
//	breaking anything.
//	To make a LEGO® analogy: LSP is like swapping a 4x2 block with a 4x2 block with a sticker on it: neither the structure’s structural integrity nor the block’s role changed; the new block only has a new
//	sticker state.

//	Covariance           and contravariance represent specific polymorphic scenarios. They allow reference types
//	to                   be converted into other types implicitly. They apply to generic type arguments, delegates, and
//	array types. Chances are, you will never need to remember this, as most of it is implicit, yet, here’s
//	an overview:
//	• Covariance (out) enables          us to use a more derived type (a subtype) instead of the supertype.
//	Covariance is usually applicable to method return types. For instance, if a base class method
//	returns an instance of a class, the equivalent method of a derived class can return an instance
//	of a subclass.
//	• Contravariance (in) is the reverse situation. It allows a less derived type (a supertype) to be
//	used instead of the subtype. Contravariance is usually applicable to method argument types.
//	If a method of a base class accepts a parameter of a particular class, the equivalent method of
//	a derived class can accept a parameter of a superclass.

