# ScriptableAnimation

I created **ScriptableAnimation** because I needed to integrate titles animation in Unity showcase videos. I could do it in a video editing software but doing everything inside Unity is better... no ? 

**ScriptableAnimation** was intended to create animations similar to what you do with video editing _Titles_.

Demo1

![demo1](https://github.com/marcbalas/ScriptableAnimation/blob/master/demo1.gif)

Another animation using the same sequence file as _Demo1_ (same animation but different objects).

Demo2

![demo3](https://github.com/marcbalas/ScriptableAnimation/blob/master/demo3.gif)

Demo3

![demo2](https://github.com/marcbalas/ScriptableAnimation/blob/master/demo2.gif)

Tweaking all the elements can be time consuming and boring so it's better if we can tune the animation at runtime and then share it with others.
Using scriptable objects make it possible.
You can create a animation template and export it as an asset file.

Then you can download the sequence asset made by the community  and drag it in your project.
Then link the elements of your scene to the animation template item.

Somehow it's a mix between the free [DOTween package](http://dotween.demigiant.com/) and Unity animation system. 
The 1st one requires compilation to test and the second cannot be shared and edited/saved at runtime. 





